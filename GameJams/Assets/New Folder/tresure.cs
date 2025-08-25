using UnityEngine;

public class TreasureDestroy : MonoBehaviour
{
    private bool hasBeenPickedUp = false;
    private bool hasBeenThrown = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Track if treasure has ever been picked up (became kinematic)
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            hasBeenPickedUp = true;
            hasBeenThrown = false; // Reset thrown status when picked up
        }

        // Mark as thrown when it becomes dynamic after being picked up
        if (hasBeenPickedUp && rb.bodyType == RigidbodyType2D.Dynamic && !hasBeenThrown)
        {
            hasBeenThrown = true;
        }
    }
}