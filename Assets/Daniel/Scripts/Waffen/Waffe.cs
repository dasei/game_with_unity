using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Waffe : MonoBehaviour {

    public float dmg;
    public float knockback;

	//number of frames the weapon strikes
	public float hitTime;

	private Movement movement;

	// Use this for initialization
	protected void Start () {
		movement = GetComponentInParent<Movement> ();
		StartCoroutine (WaitAndDestroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected IEnumerator WaitAndDestroy(){
		//Should not throw error because movement requires rigidbody2d
		movement.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		movement.moveAllowed = false;

		for (int i = 0; i < hitTime; i++) {
			yield return 0;
		}

		movement.moveAllowed = true;
		GameObject.Destroy (this.gameObject);
	}
}
