using UnityEngine;
using System.Collections;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{
    protected int hitPoints;
    protected bool isdead;

    [SerializeField]
    public int maxHitpoint = 10;

    [SerializeField]
    public int HitPoints
    {
        get
        {
            return hitPoints;
        }
    }

    [SerializeField]
    public Controller2D Controller
    {
        get
        {
            return controller;
        }
    }
    public Transform EntityRotation;

    public float Gravity
    {
        get
        {
            return gravity;
        }

        set
        {
            gravity = value;
        }
    }
    public float MaxJumpVelocity
    {
        get
        {
            return maxJumpVelocity;
        }

        set
        {
            maxJumpVelocity = value;
        }
    }
    public float MinJumpVelocity
    {
        get
        {
            return minJumpVelocity;
        }

        set
        {
            minJumpVelocity = value;
        }
    }
    public float VelocityXSmoothing
    {
        get
        {
            return velocityXSmoothing;
        }

        set
        {
            velocityXSmoothing = value;
        }
    }

    public Vector3 Velocity;
    public Vector2 DirectionalInput
    {
        get
        {
            return directionalInput;
        }

        set
        {
            directionalInput = value;
        }
    }

    public bool isJumping
    {
        get
        {
            return jumped;
        }

        set
        {
            jumped = value;
        }
    }

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;

    public float BaseMoveSpeed = 8;
    [HideInInspector]
    public float MoveSpeed;

    [HideInInspector]
    public Transform tEntity;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    Controller2D controller;

    Vector2 directionalInput;
    public bool wallSliding;
    bool jumped = true;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    internal bool isDead;


    void Start()
    {
        InitLivingEntity();
    }

    public virtual void Update()
    {
        CalculateVelocity();

        Controller.Move(Velocity * Time.deltaTime, DirectionalInput);
    }

    public virtual void InitLivingEntity()
    {
        tEntity = this.transform;
        controller = GetComponent<Controller2D>();

        MoveSpeed = BaseMoveSpeed;
        hitPoints = maxHitpoint;


        Gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        MaxJumpVelocity = Mathf.Abs(Gravity) * timeToJumpApex;
        MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Gravity) * minJumpHeight);
    }

    public void CalculateVelocity()
    {
        float targetVelocityX = DirectionalInput.x * MoveSpeed;
        Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref velocityXSmoothing, (Controller.collisions.belowPlayer) ? accelerationTimeGrounded : accelerationTimeAirborne);
        Velocity.y += Gravity * Time.deltaTime;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        DirectionalInput = input;
    }

    public void TakeDamage(DamageInfo dmg)
    {
        hitPoints -= dmg.damageAmount;

        if (HitPoints <= 0 && !isdead)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        isDead = true;
        Debug.Log(name + " has died");
        isdead = true;
    }
}
