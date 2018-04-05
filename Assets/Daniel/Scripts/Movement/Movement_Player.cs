using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBehavior))]
public class Movement_Player : Movement {
    
    private AttackBehavior attackBehavior;
    private Entity entityScript;

    //private float playerWidth, playerHeight;

	private Vector2 velocity;

	// Use this for initialization
	void Start () {        
        attackBehavior = GetComponent<AttackBehavior>();
        entityScript = GetComponent<Entity>();
    }

    public override void PerformMovement()
    {
		velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))).normalized;

        if (Input.GetKeyDown ("space")) {
			attackBehavior.SpawnWeapon (direction);
		}        
    }

	void FixedUpdate(){
		Move (velocity.x, velocity.y, true);
	}
}
