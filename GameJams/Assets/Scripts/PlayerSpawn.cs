using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private float respawnDelay = 1f;

    private Rigidbody2D rb;
    private CharacterController2D controller;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();

        // Set initial respawn position to current position
        respawnPosition = transform.position;

        // Try to find a respawn point in the scene and use it
        RespawnPoint startingPoint = Object.FindFirstObjectByType<RespawnPoint>();
        if (startingPoint != null)
        {
            respawnPosition = startingPoint.transform.position;
            // Move player to respawn point immediately when level starts
            transform.position = respawnPosition;
        }
    }

    public void Die()
    {
        // Stop player movement
        rb.linearVelocity = Vector2.zero;

        // Disable player controls to prevent input during death
        if (controller != null)
            controller.enabled = false;

        // Optional: Play death animation or effects
        Debug.Log("Player died!");

        // Reload scene after delay
        Invoke(nameof(ReloadScene), respawnDelay);
    }

    private void ReloadScene()
    {
        // Reload the current scene completely
        // This will reset everything in the level and player will start at the respawn point
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Call this to set a new respawn point
    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPosition = newRespawnPoint;
        Debug.Log($"Respawn point set to: {respawnPosition}");
    }

    // Method to manually trigger respawn (useful for testing)
    public void ForceRespawn()
    {
        Die();
    }
}