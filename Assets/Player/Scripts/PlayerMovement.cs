using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;
    private Vector2 moveVector;
    private float movementSpeed = 10.0f;
    private float rotationSpeed = 10.0f;
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

    private void OnDisable() 
    {
        if (playerControls != null)
        {
            playerControls.Disable();
            playerControls.Player.Movement.performed -= OnMovementPerformed;
            playerControls.Player.Movement.canceled -= OnMovementCanceled;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
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
            // Prevent player from strange rotation behavior
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
