using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {


    private Rigidbody2D rigidbody;
    //TODO die direction hierher auslagern

    /*
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
    */

    // Update is called once per frame
    protected void Move(float speed, float directionX, float directionY) {        
        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = (new Vector2(directionX, directionY)) * speed;
	}
}
