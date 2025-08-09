using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float jumpForce = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;

    public int maxJumpCount = 2; // 최대 점프

    public int currentJumpCount = 0; // 현재 점프 횟수

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 방향에 따라 캐릭터 뒤집기
        if (moveInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // 움직임 애니메이션 동작
        if (animator != null)
        {
            animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f);
        }

    }

    void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && currentJumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            currentJumpCount++;

            if (animator != null)
            {

                animator.SetBool("isRunning", false); // 달리기 모션 중지

                if (currentJumpCount == 1)
                {
                    animator.SetBool("isJumping", true);
                }
                else if (currentJumpCount == 2)
                {
                    animator.SetBool("isDoubleJumping", true);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") &&  collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            currentJumpCount = 0; // 바닥에 닿으면 점프 카운트 초기화

            // 점프 모션 초기화
            if (animator != null)
            {
                animator.SetBool("isGrounded", true);

                animator.SetBool("isJumping", false);

                animator.SetBool("isDoubleJumping", false);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
