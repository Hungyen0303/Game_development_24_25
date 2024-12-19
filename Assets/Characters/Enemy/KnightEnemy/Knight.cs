using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damagable))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.6f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    Animator animator;
    Damagable damagable;

    TouchingDirections touchingDirections;

    public enum WalkableDirection { Right, Left }

    private Vector2 walkDirectionVector = Vector2.right;

    private WalkableDirection walkDirection;
    public WalkableDirection WalkDirection
    {
        get
        {
            return walkDirection;
        }
        set
        {
            if (walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget {
        get 
        { 
            return _hasTarget; 
        } 
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    } 

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damagable = GetComponent<Damagable>();
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if(AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if(touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        if(!damagable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }

    private void FlipDirection()
    {
        if (walkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (walkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
