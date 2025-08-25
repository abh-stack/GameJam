using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private string nextLevelName = "Level2";
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;

    private SpriteRenderer spriteRenderer;
    private bool isUnlocked = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Debug.Log("Door unlocked! Loading next level...");

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Invoke(nameof(LoadNextLevel), 1f);
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
