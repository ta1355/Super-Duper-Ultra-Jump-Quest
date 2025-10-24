using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f; // 대시 지속 시간
    public int maxJumpCount = 2; // 2단 점프까지 가능

    [Header("Internal State")]
    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = false;
    private int currentJumpCount = 0;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector2 dashDirection; // 대시 방향 백터 값

    public Vector2 input;
    public bool isJumping;
    public bool isJumpingToFall;
    public bool isCrouchIng;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 inputDir)
    {
        if (isDashing) return; // 대시 중에는 이동 불가 (대시 누르면 앞으로 튀어나옴 그 중에는 움직이면 조금 이상해짐 그래서 대쉬 누르면 앞으로 가고 모션 끝나야 이동 가능 그러니까 대쉬 잘 써라 대신 무적기능 추가할까 생각중)
        input = inputDir; // 외부에서 입력값 받아옴

        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y); // movespeed 만큼 좌우 이동

        if (input.x > 0.01f) transform.localScale = new Vector3(1, 1, 1); // 우측 바라봄
        else if (input.x < -0.01f) transform.localScale = new Vector3(-1, 1, 1); // x값이 음수면 좌측 바라봄

        animator.SetBool("isRunning", Mathf.Abs(input.x) > 0.01f && isGrounded); //좌우 이동중이면 isrunning true 값 되면서 이동 모션 나옴 abs = 절대값
    }

    public void Jump()
    {
        if (isGrounded || currentJumpCount < maxJumpCount) // 점프 카운트가 최대 점프카운트 보다 작아야 함
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // jumpforce 만큼 위로 점프
            currentJumpCount++;
            isGrounded = false;
            isJumping = true;
            isJumpingToFall = false;

            animator.SetBool("isJumping", true);
        }
    }


    public void Dash()
    {
        if (isDashing) return; // 대시중이면 가세요라

        isDashing = true;

        dashTimer = dashDuration;

        dashDirection = new Vector2(transform.localScale.x, 0); // 대시 방향 설정 // 현재 바라보는 방향으로 대시함

        rb.linearVelocity = dashDirection * dashSpeed;

        animator.SetBool("isDashing", true);
    }


    void Update()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime; // 대시 시간 감소

            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 대시 끝나면 다시 속도 0

                animator.SetBool("isDashing", false);
            }
        }

        if (!isGrounded && isJumping && rb.linearVelocity.y < 0)
        {
            isJumping = false;
            isJumpingToFall = true;

            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpToFall", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts) // 충돌 지점들 확인 ContactPoint2D : 충돌 지점 정보 제공 함수
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                currentJumpCount = 0;

                isJumping = false;
                isJumpingToFall = false;


                animator.SetBool("isGrounded", true);
                animator.SetBool("isJumping", false);
                animator.SetBool("isJumpToFall", false);
                break; // 땋에 들어오면 더이상 확인 할 이유 없음 그러니까 탈출
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        animator.SetBool("isGrounded", false);
    }
}
