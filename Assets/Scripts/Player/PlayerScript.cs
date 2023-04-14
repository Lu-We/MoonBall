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
        inputManager    = GetComponent<inputManager>();
        stateManager    = GetComponent<stateManager>();
    }  

 
    private void FixedUpdate()
    {
        // Detect ground surface

    }
}

