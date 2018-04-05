using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float movementSpeed = 1;
    public float hp = 1;

    public float defense = 1;
    public float resistance = 1;
    public bool invincible = false;    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDamageTaken(Waffe.DamageType dmgType, float dmg)
    {
        if (this.invincible)
            return;

        float dmgTaken = 0;
        switch (dmgType)
        {
            case Waffe.DamageType.Physical:
                dmgTaken = dmg - defense;
                break;
            case Waffe.DamageType.Magic:
                dmgTaken = dmg - resistance;
                break;
        }

        hp -= Mathf.Max(1, dmgTaken);
    }
}
