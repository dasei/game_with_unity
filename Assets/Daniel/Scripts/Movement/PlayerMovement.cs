using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBahavior))]
public class PlayerMovement : Movement {
    
    public float movementSpeed = 5.0f;
    private Vector2Int direction = new Vector2Int(0,-1);
    
    private AttackBahavior attackBehavior;

    private float playerWidth, playerHeight;

	// Use this for initialization
	void Start () {        
        attackBehavior = GetComponent<AttackBahavior>();

        attackBehavior.SpawnWeapon(direction);

    }

    // Update is called once per frame
	void Update () {
        Move(movementSpeed, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}
}
