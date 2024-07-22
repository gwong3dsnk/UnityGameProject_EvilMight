using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputAction inputAction;

    void OnEnable() 
    {
        inputAction.Enable();
    }

    void OnDisable() 
    {
        inputAction.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
