using UnityEngine;

public class Warrior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public int maxJumpCount = 2;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Internal State")]
    private bool isGrounded = false;
    private int currentJumpCount = 0;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector2 dashDirection;

    private bool isCrouching = false;
    private bool attackRequested = false;

    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDashing)
        {
            HandleInput();
            HandleMovement();
            HandleJump();
            HandleCrouch();
            HandleAttack();
            UpdateAnimations();
        }

        HandleDash();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        input = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));

        // 공격 (Z키)
        if (Input.GetKeyDown(KeyCode.Z))
            attackRequested = true;

        // 대시 (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            StartDash();
    }

    void HandleMovement()
    {
        if (isCrouching) // 크러치 중엔 이동 제한
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);

        // 방향 전환
        if (input.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (input.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJump()
    {
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpPressed && (isGrounded || currentJumpCount < maxJumpCount))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            currentJumpCount++;
            isGrounded = false;

            if (animator != null)
            {
                animator.SetBool("isGrounded", false);
                if(currentJumpCount == 1)
                    animator.SetTrigger("jump");
                else
                    animator.SetTrigger("JumptoFall");
            }
        }
    }

    void HandleCrouch()
    {
        isCrouching = input.y < -0.2f && isGrounded;

        if(animator != null)
            animator.SetBool("Crouch", isCrouching);
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashDirection = new Vector2(transform.localScale.x, 0); // 바라보는 방향 대시
        rb.linearVelocity = dashDirection * dashSpeed;

        if(animator != null)
        {
            animator.SetTrigger("Dash");
            animator.SetBool("isRunning", false);
        }
    }

    void HandleDash()
    {
        if (!isDashing)
            return;

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            isDashing = false;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void HandleAttack()
    {
        if (attackRequested)
        {
            attackRequested = false;
            if (animator != null)
                animator.SetTrigger("Attack");
            // 공격 효과, 타격 판정 등 추가 가능
        }
    }

    void UpdateAnimations()
    {
        bool isRunning = Mathf.Abs(input.x) > 0.01f && isGrounded && !isCrouching && !isDashing;
        if(animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                currentJumpCount = 0;

                if (animator != null)
                    animator.SetBool("isGrounded", true);

                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;

        if (animator != null)
            animator.SetBool("isGrounded", false);
    }
}
