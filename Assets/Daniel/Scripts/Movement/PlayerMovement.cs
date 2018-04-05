using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBehavior)), RequireComponent(typeof(Entity))]
public class PlayerMovement : Movement {
    
    private AttackBehavior attackBehavior;
    private Entity entityScript;

    //private float playerWidth, playerHeight;

	private Vector2 velocity;

	// Use this for initialization
	void Start () {        
        attackBehavior = GetComponent<AttackBehavior>();
        entityScript = GetComponent<Entity>();
    }

    // Update is called once per frame
	void Update () {
		velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))).normalized;		

        if (Input.GetKeyDown ("space")) {
			attackBehavior.SpawnWeapon (direction);
		}
	}

	void FixedUpdate(){
		Move (entityScript.movementSpeed, velocity.x, velocity.y);
	}
}
