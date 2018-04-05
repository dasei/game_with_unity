using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schwert : Waffe
{
    public override DamageType GetDamageType()
    {
        return Waffe.DamageType.Physical;
    }
}
