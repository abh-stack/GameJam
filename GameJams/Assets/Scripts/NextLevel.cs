using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private string nextLevelName = "Level2";
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;

    [Header("Animation & Audio")]
    [SerializeField] private Animator animator;       
    [SerializeField] private string unlockTrigger = "Unlock"; 
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip unlockSound;   

    private SpriteRenderer spriteRenderer;
    private bool isUnlocked = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If not assigned in Inspector, try to find automatically
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        UpdateDoorAppearance();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryUnlockDoor(other.gameObject);
        }
    }

    private void TryUnlockDoor(GameObject player)
    {
        PickupAndThrow pickup = player.GetComponent<PickupAndThrow>();

        if (pickup == null)
        {
            Debug.LogWarning("Player does not have PickupAndThrow component!");
            return;
        }

        if (pickup.IsHoldingBox && pickup.currentBox != null && pickup.currentBox.CompareTag("Treasure"))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Bring the treasure to open the door!");
        }
    }

    private void OpenDoor()
    {
        if (isUnlocked) return;

        isUnlocked = true;
        UpdateDoorAppearance();

        // Trigger animation if animator is assigned
        if (animator != null && !string.IsNullOrEmpty(unlockTrigger))
        {
            animator.SetTrigger(unlockTrigger);
        }

        // Play unlock sound
        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }

        Debug.Log("Door unlocked! Loading next level...");

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Invoke(nameof(LoadNextLevel), 1.5f); // delay slightly longer for animation/sound
        }
        else
        {
            Debug.LogError("Next level name is not set!");
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    private void UpdateDoorAppearance()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isUnlocked ? unlockedColor : lockedColor;
        }
    }
}
