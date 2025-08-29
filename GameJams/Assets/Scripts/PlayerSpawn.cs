using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] private GameObject deathEffectPrefab; // Assign your particle prefab in Inspector

    private Rigidbody2D rb;
    private CharacterController2D controller;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        respawnPosition = transform.position;

        RespawnPoint startingPoint = Object.FindFirstObjectByType<RespawnPoint>();
        if (startingPoint != null)
        {
            respawnPosition = startingPoint.transform.position;
            transform.position = respawnPosition;
        }
    }

    public void Die()
    {
        rb.linearVelocity = Vector2.zero;

        if (controller != null)
            controller.enabled = false;

        // Spawn particle effect like Flappy Bird
        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // Hide player sprite before respawn
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        Debug.Log("Player died!");

        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        transform.position = respawnPosition;

        // Re-enable player
        if (controller != null)
            controller.enabled = true;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPosition = newRespawnPoint;
        Debug.Log($"Respawn point set to: {respawnPosition}");
    }

    public void ForceRespawn()
    {
        Die();
    }
}
