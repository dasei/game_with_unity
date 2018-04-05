using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Entity))]
abstract public class Movement : MonoBehaviour {


    private Entity entity;
    private new Rigidbody2D rigidbody;
	protected Vector2 direction = new Vector2(0, -1);
	private Vector2 preferredDirection = new Vector2(0,0);
	public bool moveAllowed = true;

    abstract public void PerformMovement();

    //Diese Funktion muss genau einmal pro Frame aufgerufen werden!!!!!!11elf
    protected void Move(float directionX, float directionY, bool addKnockback){
        if (this.entity == null)
            entity = GetComponent<Entity>();

        this.Move(entity.movementSpeed, directionX, directionY, addKnockback);
    }

    //Diese Funktion muss genau einmal pro Frame aufgerufen werden!!!!!!11elf
    protected void Move(float speed, float directionX, float directionY, bool addKnockback) {

        //Debug.Log("moving " + this.GetType().Name + " by: " + directionX + ", " + directionY);

        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();

        Vector2 positionTotal = new Vector2(rigidbody.position.x, rigidbody.position.y);
		
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

            positionTotal += (new Vector2(directionX, directionY)) * speed * Time.fixedDeltaTime;

            //rigidbody.velocity = (new Vector2 (directionX, directionY)) * speed;
        }

        //add knockback to total
        positionTotal += this.knockback;
        UpdateKnockback();

        //Move the Player
        rigidbody.MovePosition(positionTotal);
	}

    public Vector2 GetDirection()
    {
        return this.direction;
    }

    private Vector2 knockback = new Vector2(0,0);
    private float knockbackHalbzeit = 0.05f;
    private float knockbackMinimum = 0.01f;

    private void UpdateKnockback()
    {
        this.knockback *= Mathf.Pow(0.5f, Time.deltaTime / knockbackHalbzeit);

        if (Mathf.Abs(knockback.x) < knockbackMinimum)
            knockback.x = 0;

        if (Mathf.Abs(knockback.y) < knockbackMinimum)
            knockback.y = 0;
    }

    public void AddKnockback(Vector2 knockback)
    {
        this.knockback += knockback;
    }
}
