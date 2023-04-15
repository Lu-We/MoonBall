using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{    
    internal MovementManager movementManager;
    internal InputManager inputManager;
    internal StateManager stateManager;
    internal AudioManager audioManager;
    internal PlayerHealth playerHealth;
    public Rigidbody playerRb;
    public GameObject raquette;
    public CapsuleCollider hurtbox;

 

    
    void Start()
    {
        playerRb        = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        inputManager    = GetComponent<InputManager>();
        stateManager    = GetComponent<StateManager>();
        audioManager    = GetComponent<AudioManager>();
        playerHealth    = GetComponent<PlayerHealth>();
    }  

 
    private void FixedUpdate()
    {
        // Detect ground surface

    }
}

