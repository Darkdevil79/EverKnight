using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class WalkingEnemy : LivingEntity {

    public enum _EnemyState { FOUNDPLAYER , LOOKFOR , DEAD };

    public Animator pAnimator;
    public BaseWeapon attackWeapon;

    public Vector2 WalkIdleMinMax;
    public float AttackSpeed;
    public float AttackRange;
    public float FindPlayerRange;
    public float MoveAttackModifer;

    _EnemyState aiState;
    Transform foundPlayer;
    float animationEffectTimer;
    float attackTimer;


    public override void InitLivingEntity()
    {
        base.InitLivingEntity();

        aiState = _EnemyState.LOOKFOR;

        if (pAnimator == null)
            Debug.LogWarning("No animator found");
    }

    public override void Update()
    {
        base.Update();

        ProcessAIState();
    }

    private void ProcessAIState()
    {

        if (Common.CheckDistanceFromPlayer(tEntity) < FindPlayerRange)
        {
            foundPlayer = GameManager.Instance.MainPlayer;
            aiState = _EnemyState.FOUNDPLAYER;
        }
        else
            foundPlayer = null;


        switch (aiState)
        {
            case _EnemyState.FOUNDPLAYER:

                pAnimator.SetBool("isIdle", false);
                MoveSpeed = BaseMoveSpeed * MoveAttackModifer;

                if (Common.CheckDistanceFromPlayer(tEntity) <= AttackRange )
                {
                    pAnimator.SetBool("isAttacking", true);

                    if (attackTimer < Time.time)
                    {
                        int attackId = Random.Range(1, 3);
                        pAnimator.SetTrigger("t_Attack" + attackId);
                        attackTimer = Time.time + AttackSpeed;
                        attackWeapon.WeaponCollider.enabled = true;
                    }

                }

                break;
            case _EnemyState.LOOKFOR:

                if (!pAnimator.IsInTransition(0) && animationEffectTimer < Time.time)
                {
                    MoveSpeed = BaseMoveSpeed;
                    pAnimator.SetBool("isIdle", !pAnimator.GetBool("isIdle"));
                    animationEffectTimer = Time.time + Random.Range(WalkIdleMinMax.x, WalkIdleMinMax.y);
                }

                break;
            case _EnemyState.DEAD:
                break;
            default:
                break;
        }

        if (pAnimator.GetBool("isIdle"))
            SetDirectionalInput(new Vector2(0, 0));
        else
            SetDirectionalInput(new Vector2(-1, 0));

    }

    public override void OnDeath()
    {
        base.OnDeath();
        pAnimator.SetTrigger("t_hasDied");
        DestroyObject(this.gameObject, 3f);
    }
}
