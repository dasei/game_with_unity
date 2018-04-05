using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBehavior)), RequireComponent(typeof(Entity))]
public class PlayerMovement : Movement {
    
    private AttackBehavior attackBehavior;
    private Entity entityScript;

    //private float playerWidth, playerHeight;

	// Use this for initialization
	void Start () {        
        attackBehavior = GetComponent<AttackBehavior>();
        entityScript = GetComponent<Entity>();
    }

    // Update is called once per frame
	void Update () {
		Move (entityScript.movementSpeed, Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		if (Input.GetKeyDown ("space")) {
			attackBehavior.SpawnWeapon (direction);
		}
	}
}
