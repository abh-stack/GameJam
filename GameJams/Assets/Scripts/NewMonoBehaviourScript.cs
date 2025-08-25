using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }

    [Header("Movement Settings")]
    [SerializeField] private MoveDirection direction = MoveDirection.Horizontal;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 0f; // Optional wait at ends

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingToEnd = true;
    private float waitTimer = 0f;

    void Start()
    {
        startPos = transform.position;
        Vector3 moveDir = (direction == MoveDirection.Horizontal) ? Vector3.right : Vector3.up;
        endPos = startPos + moveDir * moveDistance;
    }

    void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        Vector3 target = movingToEnd ? endPos : startPos;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingToEnd = !movingToEnd;
            if (waitTime > 0f) waitTimer = waitTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            if (col.transform != null && gameObject.activeInHierarchy)
            {
                col.transform.SetParent(null);
            }
        }
    }

}
