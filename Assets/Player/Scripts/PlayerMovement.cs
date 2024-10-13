using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    private PlayerControls playerControls;
    private Vector2 moveVector;
    private Rigidbody rb;

    private void Awake() 
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() 
    {
        if (playerControls != null)
        {
            playerControls.Enable();
            playerControls.Player.Movement.performed += OnMovementPerformed;
            playerControls.Player.Movement.canceled += OnMovementCanceled;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
        // UpdatePlayerHeight();
    }

    private void OnDisable() 
    {
        if (playerControls != null)
        {
            playerControls.Disable();
            playerControls.Player.Movement.performed -= OnMovementPerformed;
            playerControls.Player.Movement.canceled -= OnMovementCanceled;
        }
    }

    private void MovePlayer()
    {
        // No y-axis movement required.  Up/Down (Y) should affect Forward/Backward (Z) instead.
        Vector3 movement = new Vector3(moveVector.x, 0, moveVector.y);
        
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
     
        rb.velocity = movement * movementSpeed;
        // rb.velocity = new Vector3(movement.x * movementSpeed, rb.velocity.y, movement.z * movementSpeed);
    }

    private void RotatePlayer()
    {
        if (moveVector.magnitude > 0.1f)
        {
            Vector3 direction = new Vector3(moveVector.x, 0, moveVector.y);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Prevent player from strange continuous rotation behavior
            rb.angularVelocity = Vector3.zero;
        }
    }

    // private void UpdatePlayerHeight()
    // {
    //     RaycastHit hit;
    //     // Cast a ray downwards to get the terrain height
    //     if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit))
    //     {
    //         Vector3 newPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
    //         rb.position = newPosition; // Set the new position while keeping the rotation
    //     }
    // }    

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
