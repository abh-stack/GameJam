using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private bool airControl = true;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    const float groundedRadius = 0.2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
    private float horizontal;
    private float jumpBufferTimer;

    // Public property to access grounded state
    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Check if grounded - optimized version
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        // Update jump buffer timer
        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.fixedDeltaTime;
    }

    public void Move(float move, bool jump)
    {
        horizontal = move; // Store for animator

        // Move horizontally
        if (isGrounded || airControl)
        {
            Vector3 targetVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, movementSmoothing);

            // Flip sprite
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }

        // Jump buffering
        if (jump)
            jumpBufferTimer = jumpBufferTime;

        // Jump with coyote time feel and jump buffering
       
        if (jumpBufferTimer > 0 && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset Y velocity for consistent jump height
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpBufferTimer = 0f; // Reset buffer after successful jump
        }
    }

    // Call this when jump button is released for variable jump height
    public void OnJumpUp()
    {
        if (rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("run", Mathf.Abs(horizontal) > 0.1f);
        }
    }
}