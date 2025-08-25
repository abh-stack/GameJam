using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 0.5f;

    private Vector3 originalPosition;
    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool isShaking = false;

    private void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static; // Solid platform
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(FallRoutine());
        }
    }

    private System.Collections.IEnumerator FallRoutine()
    {
        isShaking = true;

        // Shake effect
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        yield return new WaitForSeconds(fallDelay - shakeDuration);

        // Fall
        isFalling = true;
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Wait before respawn
        yield return new WaitForSeconds(respawnTime);
        Respawn();
    }

    private void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Static;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = originalPosition;
        isFalling = false;
        isShaking = false;
    }
}
