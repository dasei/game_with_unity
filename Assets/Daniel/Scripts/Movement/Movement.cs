using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {


    private new Rigidbody2D rigidbody;
	protected Vector2 direction = new Vector2(0, -1);
	private Vector2 preferredDirection = new Vector2(0,0);
	public bool moveAllowed = true;

    // Update is called once per frame
    protected void Move(float speed, float directionX, float directionY) {        
        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
		
		//Only Move if you are currently allowed to
		if (moveAllowed) {
			
			//If the player previously stood still
			if (preferredDirection.Equals (new Vector2 (0, 0)) && ((directionX != 0) || (directionY != 0))) {
				//If the player presses both keys at the same time
				if (directionX != 0 && directionY != 0) {
					//Then X is the preferred one
					preferredDirection = new Vector2 (Math.Sign(directionX), 0);
				} else {
					//Else it's the direction the player is walking in
					preferredDirection = new Vector2 (Math.Sign(directionX), Math.Sign(directionY));
				}
			//If the player already head a preferred direction
			} else {
				//If the player only moves in one direction
				if ((directionX != 0) != (directionY != 0)) {
					//That direction will become preferred
					preferredDirection = new Vector2(Math.Sign(directionX), Math.Sign(directionY));
				}
			}

			//If the player stands still
			if ((directionX == 0) && (directionY == 0)) {
				//No direction is preferred
				preferredDirection = new Vector2 (0, 0);
			}
			//If the player does not stand still
			if(!preferredDirection.Equals(new Vector2(0,0))){
				//His effective direction is the preferred one
				direction = preferredDirection;
			}

			//Move the Player
			rigidbody.MovePosition(rigidbody.position + (new Vector2 (directionX, directionY)) * speed * Time.fixedDeltaTime);
			//rigidbody.velocity = (new Vector2 (directionX, directionY)) * speed;
		}
	}
}
