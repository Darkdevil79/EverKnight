using UnityEngine;
using System.Collections;

public interface IDamageable  {

    void TakeDamage(DamageInfo dmg);


	
}

public struct DamageInfo
{
    public int damageAmount;

    public DamageInfo(int _damage)
    {
        damageAmount = _damage;
    }
}
