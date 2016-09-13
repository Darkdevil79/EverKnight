using UnityEngine;
using System.Collections;
using System;

public class ZombiePunch : BaseWeapon
{
    public override void InitWeapon()
    {
        WeaponCollider.enabled = false;
    }
}
