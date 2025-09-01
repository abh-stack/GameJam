using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Enddoor: MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private string nextLevelName = "Level2";

    [Header("UI Messages")]
    [SerializeField] private TextMeshProUGUI messageText; // Assign in Inspector
    [SerializeField] private float messageDuration = 3f;

    private bool isUnlocked = false;
    private Coroutine messageCoroutine;

    private void Awake()
    {
        // Try to find UI TextMeshPro if not assigned
        if (messageText == null)
        {
            messageText = Object.FindFirstObjectByType<TextMeshProUGUI>();
        }

        // Hide message text at start
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
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
            ShowMessage("Player does not have PickupAndThrow component!");
            return;
        }

        if (pickup.IsHoldingBox && pickup.currentBox != null && pickup.currentBox.CompareTag("Treasure"))
        {
            OpenDoor();
        }
        else
        {
            ShowMessage("Bring the treasure to open the door!");
        }
    }

    private void OpenDoor()
    {
        if (isUnlocked) return;

        isUnlocked = true;

       

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Invoke(nameof(LoadNextLevel), 0f);
        }
        else
        {
            ShowMessage("Next level name is not set!");
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    private void ShowMessage(string message)
    {
        if (messageText == null) return;

        // Stop any existing message coroutine
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        // Start new message display
        messageCoroutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        messageText.gameObject.SetActive(false);
        messageCoroutine = null;
    }
}