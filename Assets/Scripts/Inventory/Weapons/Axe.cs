using UnityEngine;
using System.Collections;
using System;

public class Axe : BaseWeapon
{
    public override void InitWeapon()
    {
        weaponTrail.sortingLayerName = "effect";
    }
}
