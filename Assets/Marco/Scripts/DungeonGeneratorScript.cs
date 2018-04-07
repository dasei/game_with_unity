using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonGeneratorScript : MonoBehaviour {

	public GameObject Wall;
	public GameObject Backgorund;
	public GameObject Player;
	public GameObject Exit;
	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public float randomFillPercent;

	public int roomThresholdSize = 12;
	public int wallThresholdSize = 12;

	int[,] map;

	struct Tile{
		public int posX;
		public int posY;

		public Tile(int x, int y){
			posX = x;
			posY = y;
		}

	}

	// Use this for initialization
	void Awake () {
		GenerateNewCave ();
		SpawnPlayer ();
		SpawnExit ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnPlayer(){
		List<List<Tile>> regions = GetRegions (0);
		List<Tile> ground = regions [0];
		int index = (int) UnityEngine.Random.Range (0, ground.Count);
		Instantiate (Player, new Vector2 (ground [index].posX, ground [index].posY), Quaternion.identity);
	}

	void SpawnExit(){
		List<List<Tile>> regions = GetRegions (0);
		List<Tile> ground = regions [0];
		int index = (int) UnityEngine.Random.Range (0, ground.Count);
		Instantiate (Exit, new Vector2 (ground [index].posX, ground [index].posY), Quaternion.identity);
	}

	void InstantiateWallObjects(){
		GameObject level = GameObject.FindGameObjectWithTag("Level");
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (map [x, y] == 1) {
					if (GetAdjacentTileCount (x, y, 0) > 0) {
						GameObject.Instantiate (Wall, new Vector2 (x, y), Quaternion.identity, level.transform);
					} else {
						GameObject.Instantiate (Backgorund, new Vector2 (x, y), Quaternion.identity, level.transform);
					}
				}
			}
		}
	}

	int GetAdjacentTileCount(int x, int y, int tileType){
		int count = 0;
		for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++) {
			for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++) {
				if (IsInMapRange (neighbourX, neighbourY)) {
					count += (map [neighbourX, neighbourY] == tileType) ? 1 : 0;
				}
			}
		}
		return count;
	}

	//Generates a new level, should be called once per scene
	void GenerateNewCave(){
		map = new int[width, height];
		FillMapRandomly ();

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}

		removeSmallRegions ();
			
	}

	void ConnectClosestRooms(List<Room> allRooms){
		int bestDistance = 0;
		Tile bestTileA = new Tile ();
		Tile bestTileB = new Tile ();
		bool possibleConnectionFound = false;

		//Loop through every combination of rooms
		foreach (Room roomA in allRooms) {
			possibleConnectionFound = false;
			foreach (Room roomB in allRooms){
				
				if (roomA == roomB) {
					continue;
				}
					
				//Loop through their edges
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
						//Calculate distance
						Tile tileA = roomA.edgeTiles [tileIndexA];
						Tile tileB = roomB.edgeTiles [tileIndexB];

						int distance = (int) ((Mathf.Pow(tileA.posX - tileB.posX, 2)) +
							(Mathf.Pow(tileA.posY - tileB.posY, 2)));

						//If either the first calculated distance or the currentyl best one
						if (distance < bestDistance || !possibleConnectionFound) {
							bestDistance = distance;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
						}
					}
				}

			}
			if (possibleConnectionFound) {
				CreatePassage (bestTileA, bestTileB);
			}
		}
	}

	void CreatePassage(Tile tileA, Tile tileB){
		List<Tile> line = GetLine (tileA, tileB);

		foreach (Tile tile in line) {
			DrawCircle (tile, 2);
		}
	}

	void DrawCircle(Tile t, int r){
		for (int x = - r; x <= + r; x++) {
			for (int y =  - r; y <= + r; y++) {
				if (((x * x) + (y * y)) <= (r * r)) {
					int drawX = t.posX + x;
					int drawY = t.posY + y;
					if (IsInMapRange (drawX, drawY)) {
						map [drawX, drawY] = 0;
					}
				}
			}
		}
	}

	//Get the Tiles which build a line with Bresenham's Algortihm
	List<Tile> GetLine(Tile from, Tile to){
		List<Tile> line = new List<Tile> ();

		int x = from.posX;
		int y = from.posY;

		int dx = to.posX - x;
		int dy = to.posY - y;

		bool inverted = false;
		int step = Math.Sign (dx);
		int gradientStep = Math.Sign (dy);

		int longest = Math.Abs (dx);
		int shortest = Math.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Math.Abs (dy);
			shortest = Math.Abs (dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add (new Tile (x, y));

			if (inverted) {
				y += step;
			} else {
				x += step;
			}

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest) {
				if (inverted) {
					x += gradientStep;
				} else {
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}

		return line;
	}

	void removeSmallRegions(){
		//Get every Region consisting of walls
		List<List<Tile>> wallRegions = GetRegions (1);

		foreach (List<Tile> region in wallRegions) {
			//If region is too small
			if (region.Count < wallThresholdSize) {
				foreach (Tile tile in region) {
					//remove it
					map [tile.posX, tile.posY] = 0;
				}
			}
		}

		//Get every Region consisting of ground
		List<List<Tile>> groundRegions = GetRegions(0);

		List<Room> allRooms = new List<Room> ();

		foreach (List<Tile> region in groundRegions) {
			//If region is too small
			if (region.Count < roomThresholdSize) {
				foreach (Tile tile in region) {
					//remove it
					map [tile.posX, tile.posY] = 1;
				}
			} else {
				//Else add it to the list of all rooms
				allRooms.Add (new Room (region, map));
			}
		}

		//Connect the rooms closest together
		while (allRooms.Count > 1) {
			ConnectClosestRooms (allRooms);
			allRooms.Clear ();
			foreach (List<Tile> region in GetRegions (0)) {
				allRooms.Add (new Room (region, map));
			}
		}

		//Instanciate all walls as Objects
		InstantiateWallObjects ();
	}

	//Fills the map completly random
	void FillMapRandomly(){
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		System.Random rng = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width || y == 0 || y == height) {
					map [x, y] = 1;
				} else {
					map [x, y] = rng.Next (0, 100) < randomFillPercent ? 1 : 0;
				}
			}
		}
	}

	//Smoothes the random map
	void SmoothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int adjacentWallCount = GetAdjacentWallCount(x,y);

				if (adjacentWallCount > 4) {
					map [x, y] = 1;
				} else if (adjacentWallCount < 4) {
					map [x, y] = 0;
				}
			}
		}
	}

	//Check if given position is inside map
	bool IsInMapRange(int x, int y){
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	//Counts all the walls adjacent to a given position
	int GetAdjacentWallCount(int x, int y){
		int count = 0;

		for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++) {
			for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++) {
				if (IsInMapRange (neighbourX, neighbourY)) {
					count += map [neighbourX, neighbourY] == 1 ? 1 : 0;
				} else {
					count += 4;
				}
			}
		}

		return count;
	}

	//Gets all Tiles belonging to a region through breadth-first search
	List<Tile> GetRegionTiles(int startX, int startY){
		//All Tiles belonging to this region
		List<Tile> tiles = new List<Tile> ();
		//Keep track of which tiles you already visited
		int[,] mapFlag = new int[width,height];
		//targetet type of Tile (Ground = 0, Wall = 1)
		int targetType = map [startX, startY];

		//Queue for breadth-first search
		Queue<Tile> queue = new Queue<Tile> ();

		//Visit starting Tile
		queue.Enqueue (new Tile (startX, startY));
		mapFlag [startX, startY] = 1;

		//While search not completed
		while (queue.Count > 0) {
			//Mark next Tile as belonging to this region
			Tile currentTile = queue.Dequeue ();
			tiles.Add (currentTile);

			//Iterate through neighbours
			for (int x = currentTile.posX - 1; x <= currentTile.posX + 1; x++) {
				for (int y = currentTile.posY - 1; y <= currentTile.posY + 1; y++) {
					//If inside Map and direct neighbour
					if (IsInMapRange (x, y) && (x == currentTile.posX || y == currentTile.posY)) {
						//If not yet visited and correct Type
						if (mapFlag [x, y] == 0 && map [x, y] == targetType) {
							//Mark this neighbour as visited and add him to the queue
							mapFlag[x,y] = 1;
							queue.Enqueue (new Tile (x, y));
						}
					}
				}
			}
		}

		return tiles;
	}

	//Returns every region in the scene as a list of tiles
	List<List<Tile>> GetRegions(int tileType){
		//List of all regions
		List<List<Tile>> regions = new List<List<Tile>>();
		//Keep track of which tiles you already visited
		int[,] mapFlag = new int[width, height];

		//Loop through every Tile
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				//If We haven't visited it yet
				if (mapFlag [x, y] == 0 && map[x,y] == tileType) {
					//Do a region-search on it
					List<Tile> currentRegion = GetRegionTiles (x, y);
					regions.Add (currentRegion);

					//Mark all tiles of this region as visited
					foreach (Tile tile in currentRegion) {
						mapFlag [tile.posX, tile.posY] = 1;
					}
				}
			}
		}

		return regions;
	}


	class Room{

		public List<Tile> tiles;
		public List<Tile> edgeTiles;
		public int roomSize;
		private int[,] level;

		public Room(){

		}

		public Room(List<Tile> roomTiles, int[,] map){
			tiles = roomTiles;
			roomSize = tiles.Count;
			level = map;

			edgeTiles = new List<Tile>();

			//For every tile
			foreach(Tile tile in tiles){
				//If adjecent to wall
				if(AdjacentWallCount(tile.posX, tile.posY) > 0){
					//then you are a edgeTile
					edgeTiles.Add(tile);
				}
			}
		}

		//Counts all the walls adjacent to a given position
		private int AdjacentWallCount(int x, int y){
			int count = 0;

			for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++) {
				for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++) {
					if (InMapRange (neighbourX, neighbourY)) {
						count += level [neighbourX, neighbourY] == 1 ? 1 : 0;
					} else {
						count += 4;
					}
				}
			}

			return count;
		}

		//Check if given position is inside map
		private bool InMapRange(int x, int y){
			return x >= 0 && x < level.GetLength(0) && y >= 0 && y < level.GetLength(1);
		}


	}
		
}
