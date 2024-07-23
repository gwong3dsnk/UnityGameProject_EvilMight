using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;
    private Vector2 moveVector;
    private float movementSpeed = 10.0f;
    private Rigidbody rb;

    private void Awake() 
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() 
    {
        playerControls.Enable();
        playerControls.Player.Movement.performed += OnMovementPerformed;
        playerControls.Player.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable() 
    {
        playerControls.Disable();
        playerControls.Player.Movement.performed -= OnMovementPerformed;
        playerControls.Player.Movement.canceled -= OnMovementCanceled;
    }

    private void FixedUpdate()
    {
        MovePlayer();
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

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
