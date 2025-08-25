using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    private Animator animator;
    float horizontalMove = 0f;
    bool jump = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Horizontal movement input
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Jump input - using GetKeyDown for jump buffering
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Only jump if not holding a box
            PickupAndThrow pickupScript = GetComponent<PickupAndThrow>();
            if (pickupScript == null || !pickupScript.IsHoldingBox)
            {
                jump = true;
                controller.OnJumpUp();
            }
        }


        // Animation updates
        animator.SetBool("run", horizontalMove != 0);
        animator.SetBool("grounded", controller.IsGrounded); // Get grounded state from controller
    }

    void FixedUpdate()
    {
        // Apply movement
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}