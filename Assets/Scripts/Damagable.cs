using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damagableHit;
    Animator animator;

    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
    private float decreaseAttack = 0.0f;
    public float DecreaseAttack
    {
        get
        {
            return decreaseAttack;
        }
        set
        {
            decreaseAttack = value;
        }
    }
    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    Damagable damagable;

    [SerializeField]
    public int health = 100;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool isAlive = true;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        damage = (int)(damage * (1 - decreaseAttack));
        Debug.Log("Damage: " + damage);
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damagableHit?.Invoke(damage, knockback);
            if (CharacterEvents.characterDamaged != null)
            {
                CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            }
            return true;
        }
        return false;
    }

    public bool Heal(int healAmount)
    {
        if (isAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healAmount);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, healAmount);
            return true;
        }
        return false;
    }
}
