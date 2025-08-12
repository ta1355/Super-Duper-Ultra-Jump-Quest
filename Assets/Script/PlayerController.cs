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

        // X축 속도만 조절 (Y축 속도는 그대로 둠)
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

        // 애니메이션 상태: 달리기
        if (animator != null)
        {
            animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f && isGrounded); // 달리기 조건: 땅에 있을 때만
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded || currentJumpCount < maxJumpCount)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // X 속도는 유지하면서 Y 속도만 변경

                currentJumpCount++; // 점프 횟수 증가

                // 애니메이션 처리
                if (animator != null)
                {
                    animator.SetBool("isRunning", false); // 달리기 중지
                    animator.SetBool("isJumping", currentJumpCount == 1); // 첫 번째 점프
                    animator.SetBool("isDoubleJumping", currentJumpCount == 2); // 두 번째 점프
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 땅에 닿을 때만
        if (collision.gameObject.CompareTag("Ground") && collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            currentJumpCount = 0; // 점프 횟수 초기화

            // 점프 상태 초기화
            if (animator != null)
            {
                animator.SetBool("isGrounded", true); // 땅에 닿았을 때
                animator.SetBool("isJumping", false); // 점프 중이 아닐 때
                animator.SetBool("isDoubleJumping", false); // 더블 점프 중이 아닐 때
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 땅을 떠나면 isGrounded를 false로 설정
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            if (animator != null)
            {
                animator.SetBool("isGrounded", false); // 공중에 있을 때
            }
        }
    }
}
