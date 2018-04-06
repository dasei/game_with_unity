using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public DungeonGeneratorScript mapGenerator;
	private GameObject player;
	private bool foundPlayer = false;
	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		if (!foundPlayer) {
			player = GameObject.FindGameObjectWithTag ("Player");
			if (player != null) {
				foundPlayer = true;
				gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -10);
			}
		} else {
			float posX, posY;
			float playerX = player.transform.position.x;
			float playerY = player.transform.position.y;
			float camHeight = 2F * cam.orthographicSize;
			float camWidth = camHeight * cam.aspect;

			//If cam is too far left
			if (playerX - (camWidth / 2) + 0.5F < 0) {
				//Stay inside
				posX = (camWidth / 2) - 0.5F;
			}
			//If cam is too far right
			else if (playerX + (camWidth / 2) -0.5F > mapGenerator.width - 1) {
				//Stay inside
				posX = mapGenerator.width - (camWidth/2) - 1F + 0.5F;
			}
			//If cam image is inside cave
			else {
				//Just follow the player
				posX = playerX;
			}

			//If cam is too low
			if (playerY - (camHeight / 2) + 0.5F < 0) {
				//Stay inside
				posY = (camHeight / 2) - 0.5F;
			}
			//If cam is too high
			else if (playerY + (camHeight / 2) - 0.5F > mapGenerator.height - 1) {
				//Stay inside
				posY = mapGenerator.height - (camHeight/2) - 1F + 0.5F;
			}
			//If cam image is inside cave
			else {
				//Just follow the player
				posY = playerY;
			}

			cam.transform.position = new Vector3 (posX, posY, -10);
		}
	}
}
