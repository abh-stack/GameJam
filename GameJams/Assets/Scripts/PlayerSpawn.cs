using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] private GameObject fallingSpritePrefab;
    [SerializeField] private AudioSource deathSound;

    private Rigidbody2D rb;
    private CharacterController2D controller;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Die()
    {
        // Prevent multiple death calls
        if (isDead) return;
        isDead = true;

        // Stop player movement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Disable player controller
        if (controller != null)
            controller.enabled = false;

        // Create falling sprite effect
        if (fallingSpritePrefab != null && spriteRenderer != null)
        {
            GameObject fallingCopy = Instantiate(fallingSpritePrefab, transform.position, Quaternion.identity);

            SpriteRenderer copyRenderer = fallingCopy.GetComponent<SpriteRenderer>();
            if (copyRenderer != null)
            {
                copyRenderer.sprite = spriteRenderer.sprite;
                copyRenderer.flipX = spriteRenderer.flipX;
            }

            Rigidbody2D rb2d = fallingCopy.GetComponent<Rigidbody2D>();
            if (rb2d != null)
                rb2d.angularVelocity = Random.Range(-200f, 200f);

            Destroy(fallingCopy, 3f);
        }

        // Hide the player sprite
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // Play death sound
        if (deathSound != null)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(deathSound.clip);
            else
                AudioSource.PlayClipAtPoint(deathSound.clip, transform.position);
        }

        Debug.Log("Player died!");

        // Schedule scene reload (instead of manual respawn)
        Invoke(nameof(ReloadScene), respawnDelay);
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        // Only useful if using checkpoint respawn
        Debug.Log($"Respawn point set to: {newRespawnPoint}");
    }
}
