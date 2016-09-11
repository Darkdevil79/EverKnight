using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(Controller2D))]
public class Player : LivingEntity {

    public Animator pAnimator;
    [HideInInspector]
    public PlayerCustomizer PlayerCustomizer;
    [HideInInspector]
    public PlayerCombat PlayerCombat;

    public Transform FloorEffectSpawn;


    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;

    float timeToWallUnstick;
    int wallDirX;

    public override void InitLivingEntity()
    {
        base.InitLivingEntity();

        PlayerCustomizer = GetComponent<PlayerCustomizer>();
        PlayerCombat = GetComponent<PlayerCombat>();

        if (pAnimator == null)
            Debug.LogWarning("No animator found");
    }

    public override void Update()
    {
        HandleWallSliding();

        base.Update();

        if (DirectionalInput.x != 0 && Controller.collisions.belowPlayer)
            pAnimator.SetFloat("moveSpeed", 1);
        else
            pAnimator.SetFloat("moveSpeed", 0);

        if (Controller.collisions.abovePlayer || Controller.collisions.belowPlayer)
        {
            Velocity.y = 0;
        }

        if (Controller.collisions.isGrounded && isJumping && pAnimator.GetCurrentAnimatorStateInfo(0).IsName("falling"))
        {
            isJumping = false;
            Instantiate(Resources.Load("Effects/CFXM2_GroundRockHit Gray"), FloorEffectSpawn, false);
        }

        if (Controller.collisions.faceDir == 1) // Face Left
            EntityRotation.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (Controller.collisions.faceDir == -1)
            EntityRotation.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

        pAnimator.SetBool("isGrounded", Controller.collisions.isGrounded);
        pAnimator.SetBool("isWallSliding", wallSliding);

    }

    public void OnJumpInputDown()
    {

        if (wallSliding)
        {
            if (wallDirX == DirectionalInput.x)
            {
                Velocity.x = -wallDirX * wallJumpClimb.x;
                Velocity.y = wallJumpClimb.y;
            }
            else if (DirectionalInput.x == 0)
            {
                Velocity.x = -wallDirX * wallJumpOff.x;
                Velocity.y = wallJumpOff.y;
            }
            else
            {
                Velocity.x = -wallDirX * wallLeap.x;
                Velocity.y = wallLeap.y;
            }
        }

        if (Controller.collisions.belowPlayer)
        {
            Velocity.y = MaxJumpVelocity;
        }

        isJumping = true;
        pAnimator.SetTrigger("t_startJump");
    }

    public void OnJumpInputUp()
    {
        if (Velocity.y > MinJumpVelocity)
        {
            Velocity.y = MinJumpVelocity;
        }
    }

    public void DrinkPosion()
    {
        pAnimator.SetTrigger("t_DrinkPosion");
    }

    void HandleWallSliding()
    {
        bool allowWallSliding = true;
        wallSliding = false;
        wallDirX = (Controller.collisions.playerLeft) ? -1 : 1;

        //Check if it is a wall not an enemy
        if (wallDirX == -1 && Controller.collisions.hitPlayerLeft != null)
        {
            if (Controller.collisions.hitPlayerLeft.gameObject.layer == LayerMask.NameToLayer(GameManager.Instance.EnemyLayer))
                allowWallSliding = false;
        }
        else if (wallDirX == 1 && Controller.collisions.hitPlayerRight != null)
        {
            if (Controller.collisions.hitPlayerRight.gameObject.layer == LayerMask.NameToLayer(GameManager.Instance.EnemyLayer))
                allowWallSliding = false;
        }

        if (allowWallSliding)
        {
       
            if ((Controller.collisions.playerLeft || Controller.collisions.playerRight) && !Controller.collisions.belowPlayer && Velocity.y < 0)
            {
                wallSliding = true;

                if (Velocity.y < -wallSlideSpeedMax)
                {
                    Velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0)
                {
                    VelocityXSmoothing = 0;
                    Velocity.x = 0;

                    if (DirectionalInput.x != wallDirX && DirectionalInput.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }
        }
    }

   
}
