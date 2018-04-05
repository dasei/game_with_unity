using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Player : Entity {

    public override void OnDeath()
    {
        //TODO
    }

    public override void Update()
    {
        GetComponent<Movement>().PerformMovement();
    } 
}
