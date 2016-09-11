using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public float TimeBetweenAttack;
    public BaseWeapon CurrentWeapon;
    
    Player playerController;

    float lastAttack;
    int attackID;

    void Start()
    {
        //Teset Sync

        playerController = GetComponent<Player>();
        attackID = 1;
        AttachNewWeapon(CurrentWeapon);
    }

    void Update()
    {
        if ((lastAttack + TimeBetweenAttack) < Time.time)
        {
            attackID = 1;
        }
    }

    void AttachNewWeapon(BaseWeapon newWeapon)
    {
        CurrentWeapon = newWeapon;
        CurrentWeapon.WeaponCollider.enabled = false;
    }

    public void SendAttackTigger()
    {
        if (attackID == 1)
        {
            playerController.pAnimator.SetTrigger("t_StartAttack" + attackID);
            lastAttack = Time.time;
            attackID++;
        }
        else if ((lastAttack + TimeBetweenAttack) > Time.time)
        {
            playerController.pAnimator.SetTrigger("t_StartAttack" + attackID);
            lastAttack = Time.time;
            attackID++;

            if (attackID > 3)
                attackID = 1;
        }

        CurrentWeapon.WeaponCollider.enabled = true;

    }
}



