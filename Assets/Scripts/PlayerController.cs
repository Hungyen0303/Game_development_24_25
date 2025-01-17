using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damagable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 5f;
    private PlayerInput playerInput;
    private InputAction axeAction;
    private InputAction thuongAction;

    Vector2 moveInput;

    TouchingDirections touchingDirections;
    Damagable damagable;
    public LayerMask interactableLayer;

    UIManager uiManager;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
    }

    [SerializeField]
    private bool isMoving = false;

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool isRunning = false;

    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        set
        {
            isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        private set
        {
            if (isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            isFacingRight = value;
        }

    }
    private void Update()
    {
        CheckDeathZone();

        handleUpdate();
    }
    [SerializeField] private const float deathY = -20f; // Độ cao mà nhân vật sẽ chết
    private Vector3 spawnPoint; // Điểm hồi sinh ban đầu
    private Vector3 checkpoint1 = new Vector3(32, -4, 0); // Điểm hồi sinh ban đầu
    private Vector3 checkpoint2 = new Vector3(85.6f, -6.2f, 0); // Điểm hồi sinh ban đầu
    private Vector3 checkpoint3 = new Vector3(155, -2.5f, 0); // Điểm hồi sinh ban đầu

    private void CheckDeathZone()
    {
        if (transform.position.y < deathY)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (transform.position.x > checkpoint3.x)
            transform.position = checkpoint3;
        else if (transform.position.x > checkpoint2.x)
            transform.position = checkpoint2;
        else if (transform.position.x > checkpoint1.x)
            transform.position = checkpoint1;
        else
            transform.position = spawnPoint;
    }



    Rigidbody2D rb;

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damagable = GetComponent<Damagable>();

        uiManager = FindObjectOfType<UIManager>();
        playerInput = GetComponent<PlayerInput>();
        axeAction = playerInput.actions["riuAttack"]; // Giả sử action map có tên "UseAxe"
        axeAction.Disable();
        thuongAction = playerInput.actions["thuongAttack"]; // Giả sử action map có tên "UseAxe"
        thuongAction.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        // notificationPanel.SetActive(false);

    }

    // Update is called once per frame
    public void handleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            interactWithNPC();
        }
    }

    private void interactWithNPC()
    {
        var facingDir = new Vector2(1, 0) * (isFacingRight ? 1 : -1);
        // Debug.Log(facingDir);
        var interactPos = (Vector2)transform.position + facingDir;
        // Debug.DrawLine(transform.position,interactPos,Color.red,1f);
        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
    private void FixedUpdate()
    {
        if (!damagable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        // If key p down, will open gameover screen for demo
        if (Input.GetKeyDown(KeyCode.P))
        {
            uiManager.ShowGameOverScreen();
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            // check xem co dung phai NPC khong
            if (IsMoving)
            {
                Vector2 targetPos = (Vector2)transform.position + moveInput;
                if (Physics2D.OverlapCircle(targetPos, 0.2f, interactableLayer) != null)
                {
                    IsMoving = false;
                }
            }
            SetFacingDirection(moveInput);
        }

        else
        {
            IsMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }

    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);

        }
    }
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }
    public void OnRiuAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.RiuAttackTrigger);
        }
    }
    public void OnThuongAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.ThuongAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    public GameObject notificationPanel; // Thông báo nhỏ ở góc màn hình
    private GameObject currentWeapon; // Lưu vũ khí hiện tại
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("armor"))
        {
            Debug.Log("Here");
            currentWeapon = collision.gameObject;
            damagable.DecreaseAttack = 0.2f;
            //notificationPanel.SetActive(true);
            //notificationText.text = "Press 1 to use this new weapon";
            Destroy(currentWeapon);

            //StartCoroutine(HideNotificationAfterDelay(2f));

        }

        if (collision.CompareTag("shoe"))
        {
            currentWeapon = collision.gameObject;
            //notificationPanel.SetActive(true);
            //notificationText.text = "Press 1 to use this new weapon";
            Destroy(currentWeapon);
            walkSpeed *= 1.2f;
            runSpeed *= 1.2f;
            airWalkSpeed *= 1.2f;
            Debug.Log("Speed increased");
            StartCoroutine(ResetSpeedsAfterDelay(10f));

        }

        if (collision.CompareTag("riu"))
        {
            currentWeapon = collision.gameObject;
            //notificationPanel.SetActive(true);
            //notificationText.text = "Press 1 to use this new weapon";
            Destroy(currentWeapon);
            axeAction.Enable();



        }

        if (collision.CompareTag("thuong"))
        {
            currentWeapon = collision.gameObject;
            //notificationPanel.SetActive(true);
            //notificationText.text = "Press 1 to use this new weapon";
            Destroy(currentWeapon);
            thuongAction.Enable();



        }
    }
    private IEnumerator ResetSpeedsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Reset to original speeds
        walkSpeed = 5f;
        runSpeed = 8f;
        airWalkSpeed = 5f;
    }

    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationPanel.SetActive(false);

    }
}
