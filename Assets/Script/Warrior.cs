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
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool isJumpToFall = false;

    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();

        HandleMovement();
        HandleJump();
        HandleCrouch();

        HandleDash();
        HandleAttack();
        UpdateAnimations();
    }

    // 사용자 입력 처리(화살표로 이동, z키 공격, left shift 대시)
    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        input = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
            attackRequested = true;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isAttacking)
            StartDash();
    }

    // 사용자 입력에 따른 이동 처리
    void HandleMovement()
    {
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);

        // 좌우 방향 전환
        if (input.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (input.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // 점프 처리(화살표 위 or 스페이스바)
    void HandleJump()
    {
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpPressed && (isGrounded || currentJumpCount < maxJumpCount))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            currentJumpCount++;
            isGrounded = false;
            isJumping = true;
            isJumpToFall = false;

            if (animator != null)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isJumpToFall", false);
                animator.SetBool("isGrounded", false);
            }
        }
    }
    // 않기 처리(아직 구현중...)
    void HandleCrouch()
    {
        isCrouching = input.y < -0.2f && isGrounded;

        if (animator != null)
            animator.SetBool("Crouch", isCrouching);
    }

    // 대시 시작
    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashDirection = new Vector2(transform.localScale.x, 0);
        rb.linearVelocity = dashDirection * dashSpeed;

        if (animator != null)
            animator.SetBool("isDashing", true);
    }

    // 대시 처리
    void HandleDash()
    {
        if (!isDashing)
            return;

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            isDashing = false;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (animator != null)
                animator.SetBool("isDashing", false);
        }
    }

    // 공격 처리
    void HandleAttack()
    {
        if (attackRequested && !isAttacking)
        {
            attackRequested = false;
            isAttacking = true;

            if (animator != null)
                animator.SetBool("isAttacking", true);

            // 공격 중 이동 가능하도록 주석 처리     <- 어떻게 할지 생각중(이동하면서 공격가능하게 하는게 맞나 아니면 공격하면 제자리에만 있어야하나...)
            // rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    // 공격 리셋(에니메이션에서 함수 호출해서 사용중)
    public void ResetAttack()
    {
        isAttacking = false;

        if (animator != null)
            animator.SetBool("isAttacking", false);
    }

    // 점프 → 점프투폴 상태 전환용 (애니메이션 이벤트에서 호출)
    public void SetJumpToFall()
    {
        isJumpToFall = true;
        isJumping = false;

        if (animator != null)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpToFall", true);
        }
    }

    // 착지 시점에 호출 (애니메이션 이벤트 or 충돌에서)
    public void ResetJumpStates()
    {
        isJumping = false;
        isJumpToFall = false;

        if (animator != null)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpToFall", false);
            animator.SetBool("isGrounded", true);
        }
    }

    // 에니메이션 업데이트
    void UpdateAnimations()
    {
        bool isRunning = Mathf.Abs(input.x) > 0.01f && isGrounded && !isCrouching && !isDashing;

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    // 충돌 감지
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                currentJumpCount = 0;
                ResetJumpStates();

                if (animator != null)
                    animator.SetBool("isGrounded", true);

                break;
            }
        }
    }

    // 충돌 종료 감지
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;

        if (animator != null)
            animator.SetBool("isGrounded", false);
    }
}
