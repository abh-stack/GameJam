using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private bool airControl = true;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landingClip;
    [SerializeField][Range(0f, 1f)] private float jumpVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float landingVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField] private float minFallVelocityForLandingSound = -2f; // Minimum fall velocity to trigger landing sound

    private AudioSource audioSource;

    const float groundedRadius = 0.2f;
    private bool isGrounded;
    private bool wasGrounded;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
    private float horizontal;
    private float jumpBufferTimer;

    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = false;
        audioSource.playOnAwake = false;
        UpdateAudioSourceVolume();
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        // Check for landing
        if (!wasGrounded && isGrounded)
        {
            // Only play landing sound if the character was falling with sufficient velocity
            if (rb.linearVelocity.y <= minFallVelocityForLandingSound)
            {
                PlayLandingSound();
            }
        }

        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("run", Mathf.Abs(horizontal) > 0.1f);
        }
    }

    public void Move(float move, bool jump)
    {
        horizontal = move;

        if (isGrounded || airControl)
        {
            Vector3 targetVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, movementSmoothing);

            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }

        if (jump)
            jumpBufferTimer = jumpBufferTime;

        if (jumpBufferTimer > 0 && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpBufferTimer = 0f;
            PlayJumpSound();
        }
    }

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

    private void PlayJumpSound()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip, jumpVolume * masterVolume);
        }
    }

    private void PlayLandingSound()
    {
        if (landingClip != null)
        {
            audioSource.PlayOneShot(landingClip, landingVolume * masterVolume);
        }
    }

    private void UpdateAudioSourceVolume()
    {
        if (audioSource != null)
        {
            audioSource.volume = masterVolume;
        }
    }

    // Public methods for runtime volume control
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAudioSourceVolume();
    }

    public void SetJumpVolume(float volume)
    {
        jumpVolume = Mathf.Clamp01(volume);
    }

    public void SetLandingVolume(float volume)
    {
        landingVolume = Mathf.Clamp01(volume);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetJumpVolume()
    {
        return jumpVolume;
    }

    public float GetLandingVolume()
    {
        return landingVolume;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
        }
    }

    // Called automatically when values change in the inspector
    private void OnValidate()
    {
        if (audioSource != null)
        {
            UpdateAudioSourceVolume();
        }
    }
}