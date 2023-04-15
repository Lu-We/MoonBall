using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{    
    internal MovementManager movementManager;
    internal InputManager inputManager;
    internal StateManager stateManager;
    public Rigidbody playerRb;

 

    
    void Start()
    {
        playerRb        = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        inputManager    = GetComponent<InputManager>();
        stateManager    = GetComponent<StateManager>();
    }  

 
    private void FixedUpdate()
    {
        // Detect ground surface

    }
}

