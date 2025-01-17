using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damagable))]
public class Boss3Controller : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.6f;
    public DetectionZone attackZone;
    public Transform playerTransform;
    public float detectionRange = 5f;

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

    public bool HasTarget
    {
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
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);
        if (!damagable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                MoveTowardsPlayer();
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

    
        if (distanceToPlayer > 0.1f)
        {
            rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x + (walkAcceleration * directionToPlayer.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                rb.velocity.y
            );

            if (directionToPlayer.x > 0 && walkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if (directionToPlayer.x < 0 && walkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    void Start()
    {
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            if (walkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if (walkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
        }
    }
}
