using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class WalkingEnemy : LivingEntity {

    public enum _EnemyState { FOUNDPLAYER , ATTACKPLAYER ,  LOOKFOR , DEAD };

    public Animator pAnimator;
    public BaseWeapon attackWeapon;

    public Vector2 WalkIdleMinMax;
    public float AttackSpeed;
    public float AttackRange;
    public float FindPlayerRange;
    public float MoveAttackModifer;

    public _EnemyState aiState;
    Transform foundPlayer;
    float animationEffectTimer;
    float attackTimer;
    float dstFromPlayer;


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

        dstFromPlayer = Common.CheckDistanceFromPlayer(tEntity);

        ProcessAIState();
    }

    private void ProcessAIState()
    {

        if (dstFromPlayer < FindPlayerRange && aiState == _EnemyState.LOOKFOR)
        {
            foundPlayer = GameManager.Instance.MainPlayer;
            aiState = _EnemyState.FOUNDPLAYER;
        }
        else
            foundPlayer = null;


        switch (aiState)
        {
            case _EnemyState.LOOKFOR:

                if (!pAnimator.IsInTransition(0) && animationEffectTimer < Time.time)
                {
                    MoveSpeed = BaseMoveSpeed;
                    pAnimator.SetBool("isIdle", !pAnimator.GetBool("isIdle"));
                    animationEffectTimer = Time.time + Random.Range(WalkIdleMinMax.x, WalkIdleMinMax.y);
                }

                if (pAnimator.GetBool("isIdle"))
                    SetDirectionalInput(new Vector2(0, 0));
                else
                    SetDirectionalInput(new Vector2(-1, 0));

                break;

            case _EnemyState.FOUNDPLAYER:

                pAnimator.SetBool("isIdle", false);
                MoveSpeed = BaseMoveSpeed * MoveAttackModifer;

                Debug.Log(dstFromPlayer);

                if (dstFromPlayer <= AttackRange )
                {
                    aiState = _EnemyState.ATTACKPLAYER;
                }

                break;

            case _EnemyState.ATTACKPLAYER:

                pAnimator.SetBool("isAttacking", true);
                SetDirectionalInput(new Vector2(0, 0));

                if (attackTimer < Time.time)
                {
                    attackWeapon.WeaponCollider.enabled = true;
                    int attackId = Random.Range(1, 3);
                    pAnimator.SetTrigger("t_Attack" + attackId);
                    attackTimer = Time.time + AttackSpeed;
         
                }

                if (dstFromPlayer > AttackRange && dstFromPlayer < FindPlayerRange)
                {
                    pAnimator.SetBool("isAttacking", false);
                    aiState = _EnemyState.LOOKFOR; 
                }

                break;
           
            case _EnemyState.DEAD:
                break;
            default:
                break;
        }


       

    }

    public override void EntityTakeHit(Vector2 hitPos)
    {
        Instantiate(Resources.Load("Effects/BloodHit"), hitPos, Quaternion.identity);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        pAnimator.SetTrigger("t_hasDied");
        entityCollider.enabled = false;
        DestroyObject(this.gameObject, 3f);
    }
}
