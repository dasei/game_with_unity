using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBehavior))]
public class PlayerMovement : Movement {
    
    public float movementSpeed = 5.0f;
    
    private AttackBehavior attackBehavior;

    private float playerWidth, playerHeight;

	private Vector2 velocity;

	// Use this for initialization
	void Start () {        
        attackBehavior = GetComponent<AttackBehavior>();

    }

    // Update is called once per frame
	void Update () {
		velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))).normalized;
		Move (movementSpeed, Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		if (Input.GetKeyDown ("space")) {
			attackBehavior.SpawnWeapon (direction);
		}
	}

	void FixedUpdate(){
		Move (movementSpeed, velocity.x, velocity.y);
	}
}
