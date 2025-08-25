using UnityEngine;

public class PickupAndThrow : MonoBehaviour
{
    [SerializeField] private float pickupRange = 1.5f;
    [SerializeField] private float throwForce = 15f;

    private GameObject currentBox;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentBox == null) TryPickup();
            else Drop();
        }

        if (Input.GetKeyDown(KeyCode.C) && currentBox != null) Throw();

        if (currentBox != null)
        {
            float xOffset = transform.localScale.x > 0 ? 0.8f : -0.8f;
            currentBox.transform.position = transform.position + Vector3.up * 0.5f + Vector3.right * xOffset;
        }
    }

    private void TryPickup()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, pickupRange);
        foreach (var item in items)
        {
            if (item.CompareTag("PickupableBox") || item.CompareTag("Treasure"))
            {
                currentBox = item.gameObject;
                item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                item.GetComponent<Collider2D>().enabled = false;
                break;
            }
        }
    }

    private void Drop()
    {
        var rb = currentBox.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        currentBox.GetComponent<Collider2D>().enabled = true;
        currentBox = null;
    }

    private void Throw()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        var rb = currentBox.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * throwForce + Vector2.up * 5f, ForceMode2D.Impulse);
        currentBox.GetComponent<Collider2D>().enabled = true;
        currentBox = null;
    }

    // Public property to check if holding treasure specifically
    public bool IsHoldingBox => currentBox != null && currentBox.CompareTag("Treasure");
}