using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour {

    public float movementSpeed = 4;
    public float hp = 10;

    public float defense = 3;
    public float resistance = 1;
    public bool immuneToDamage = false;
    public float immunityToDamageCooldownSeconds = 1.5f;

    public bool immuneToKnockback = false;
    public float immunityToKnockbackCooldownSeconds = 0.25f;

    private Movement movementScript;

    private void Start()
    {
        movementScript = GetComponent<Movement>();
    }

    abstract public void Update();

    abstract public void OnDeath();

    public void OnDamageTaken(Waffe.DamageType dmgType, float dmg)
    {
        if (this.immuneToDamage)
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

        StartCoroutine(ImmunityToDamageCooldown(immunityToDamageCooldownSeconds));
    }

    public void OnKnockbackReceived(Vector2 direction, float force)
    {
        if (this.immuneToKnockback || this.movementScript == null)
            return;

        //GetComponent<Rigidbody2D>().AddForce(direction * force);

        
        movementScript.AddKnockback(direction * force);

        StartCoroutine(ImmunityToKnockbackCooldown(immunityToKnockbackCooldownSeconds));
    }

    //Cooldowns

    private IEnumerator ImmunityToDamageCooldown(float timeInSeconds)
    {
        this.immuneToDamage = true;

        float timePassed = 0;
        while ((timePassed += Time.deltaTime) < timeInSeconds)
            yield return 0;

        this.immuneToDamage = false;        
    }

    private IEnumerator ImmunityToKnockbackCooldown(float timeInSeconds)
    {
        this.immuneToKnockback = true;

        float timePassed = 0;
        while ((timePassed += Time.deltaTime) < timeInSeconds)
            yield return 0;

        this.immuneToKnockback = false;
    }
}
