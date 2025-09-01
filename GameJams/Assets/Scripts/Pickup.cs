using UnityEngine;

public class PickupAndThrow : MonoBehaviour
{
    [Header("Pickup Settings")]
    private float pickupRange = 1.75f;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private LayerMask pickupLayerMask = -1; // What layers can be picked up

    [Header("Audio Settings")]
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private float audioVolume = 0.08f; // Made SerializeField for easier tweaking

    public GameObject currentBox;
    private AudioSource audioSource;
    private Rigidbody2D currentBoxRb; // Cache the rigidbody reference

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentBox == null) TryPickup();
            else Drop();
        }
        if (Input.GetKeyDown(KeyCode.E) && currentBox != null) Throw();
        UpdateHeldObjectPosition();
    }

    private void UpdateHeldObjectPosition()
    {
        if (currentBox != null)
        {
            float xOffset = transform.localScale.x > 0 ? 0.8f : -0.8f;
            currentBox.transform.position = transform.position + Vector3.up * 0.5f + Vector3.right * xOffset;
        }
    }

    private void TryPickup()
    {
        // Create box in front of and below the player
        Vector2 boxSize = new Vector2(pickupRange, pickupRange * 0.8f); // Width x Height
        float xOffset = transform.localScale.x > 0 ? pickupRange * 0.5f : -pickupRange * 0.5f; // Forward direction
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(xOffset, -0.3f); // Forward and slightly down

        Collider2D[] items = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, pickupLayerMask);
        foreach (var item in items)
        {
            if (item.CompareTag("PickupableBox") || item.CompareTag("Treasure"))
            {
                currentBox = item.gameObject;
                currentBoxRb = item.GetComponent<Rigidbody2D>(); // Cache the reference
                currentBoxRb.bodyType = RigidbodyType2D.Kinematic;
                item.GetComponent<Collider2D>().enabled = false;
                PlayPickupSound();
                break;
            }
        }
    }

    private void Drop()
    {
        if (currentBoxRb != null)
        {
            currentBoxRb.bodyType = RigidbodyType2D.Dynamic;
            // Clear any accumulated forces
            currentBoxRb.angularVelocity = 0f;
            currentBox.GetComponent<Collider2D>().enabled = true;
        }
        ResetCurrentBox();
    }

    private void Throw()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        if (currentBoxRb != null)
        {
            currentBoxRb.bodyType = RigidbodyType2D.Dynamic;
            // Clear accumulated forces first
            currentBoxRb.angularVelocity = 0f;
            currentBox.GetComponent<Collider2D>().enabled = true;

            // Apply clean throw force
            Vector2 throwVector = direction * throwForce + Vector2.up * 5f;
            currentBoxRb.AddForce(throwVector, ForceMode2D.Impulse);
        }
        ResetCurrentBox();
    }

    private void ResetCurrentBox()
    {
        currentBox = null;
        currentBoxRb = null;
    }

    private void PlayPickupSound()
    {
        if (pickupClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupClip, audioVolume);
        }
    }

    public bool IsHoldingBox => currentBox != null && currentBox.CompareTag("Treasure");
}