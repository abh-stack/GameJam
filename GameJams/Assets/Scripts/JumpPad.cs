using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce = 20f;
    [SerializeField] private float bounceCooldown = 0.2f;

    private Animator animator;
    private float lastBounceTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player and cooldown has passed
        if (other.CompareTag("Player") && Time.time - lastBounceTime >= bounceCooldown)
        {
            BouncePlayer(other);
        }
    }

    private void BouncePlayer(Collider2D player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            // Reset Y velocity and apply bounce force
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
            playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

            // Trigger bounce animation if available
            if (animator != null)
            {
                animator.SetTrigger("bounce");
            }

            // Set cooldown
            lastBounceTime = Time.time;
        }
    }
}