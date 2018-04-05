using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
abstract public class Waffe : MonoBehaviour {

    public float dmg;
    public DamageType dmgType;
    public float knockback;
    private static float globalKnockbackMultiplier = 0.25f;

	//number of frames the weapon strikes
	public float hitTime;

	private Movement movement;

    abstract public DamageType GetDamageType();
    public enum DamageType {Physical, Magic};

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

    //<summary>
    //
    //</summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();

        if (entity != null)
        {
            //knockback entity
            entity.OnKnockbackReceived(movement.GetDirection(), knockback * globalKnockbackMultiplier);

            //damage entity
            entity.OnDamageTaken(this.GetDamageType(), dmg);
        }
    }

}
