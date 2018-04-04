using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBehavior))]
public class PlayerMovement : Movement {
    
    public float movementSpeed = 5.0f;
    
    private AttackBehavior attackBehavior;

    private float playerWidth, playerHeight;

	// Use this for initialization
	void Start () {        
        attackBehavior = GetComponent<AttackBehavior>();

    }

    // Update is called once per frame
	void Update () {
		Move (movementSpeed, Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		if (Input.GetKeyDown ("space")) {
			attackBehavior.SpawnWeapon (direction);
		}
	}
}
