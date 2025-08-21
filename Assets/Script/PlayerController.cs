using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float jumpForce = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;

    public int maxJumpCount = 2;
    private int currentJumpCount = 0;

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

        if (moveInput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (animator != null)
            animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f && isGrounded);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded || currentJumpCount < maxJumpCount)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                currentJumpCount++;

                if (animator != null)
                {
                    animator.SetBool("isRunning", false);
                    animator.SetTrigger(currentJumpCount == 1 ? "Jump" : "DoubleJump");
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥 판정: Y 방향 충돌이 위쪽일 경우 (0.7f 이상)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                currentJumpCount = 0;

                if (animator != null)
                {
                    animator.SetBool("isGrounded", true);
                }

                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;

        if (animator != null)
        {
            animator.SetBool("isGrounded", false);
        }
    }
}
