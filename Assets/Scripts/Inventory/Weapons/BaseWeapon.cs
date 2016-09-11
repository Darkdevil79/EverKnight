using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PolygonCollider2D))]
public abstract class BaseWeapon : MonoBehaviour {

    public string WeaponName;
    public GameItem WeaponData;
    public TrailRenderer weaponTrail;

    public int MinWeaponAttack;
    public int MaxWeaponAttack;

    public bool OneShotDamage;

    [HideInInspector]
    public PolygonCollider2D WeaponCollider;

    bool hasAppliedDamage = false;
    float damageApplyCoolDown = 0;
    DamageInfo weaponDamageInfo;


    void Start()
    {
        WeaponCollider = GetComponent<PolygonCollider2D>();
        InitWeapon();
    }

    public abstract void InitWeapon();

    void OnTriggerEnter2D (Collider2D hitObject)
    {
        LivingEntity hitEntiy = hitObject.GetComponent<LivingEntity>();
        if (hitEntiy == null) return;

        if (hitObject.gameObject != gameObject)
        {
            if (!hasAppliedDamage)
            {
                ApplyDamageToHitObject(hitEntiy);
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        ResetApplyDamageTigger();
    }

    public void ResetApplyDamageTigger()
    {
        hasAppliedDamage = false;
    }

    public virtual void ApplyDamageToHitObject(LivingEntity hitEntity)
    {
        weaponDamageInfo = CalculateWeaponDamage(hitEntity);
        hitEntity.TakeDamage(weaponDamageInfo);
        WeaponCollider.enabled = false;
    }

    public virtual DamageInfo CalculateWeaponDamage(LivingEntity hitEntity)
    {
        int dmg = UnityEngine.Random.Range(MinWeaponAttack, MaxWeaponAttack + 1);

        return new DamageInfo(dmg);
    }
}
