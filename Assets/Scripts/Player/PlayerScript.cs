using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{    
    [SerializeField]
    private int playerIndex = 0;

    internal MovementManager movementManager;
    internal InputManager inputManager;
    internal StateManager stateManager;
    internal AudioManager audioManager;
    internal PlayerHealth playerHealth;
    public Rigidbody playerRb;
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

    public int GetPlayerIndex(){
        return playerIndex;
    }
    private void FixedUpdate()
    {
        // Detect ground surface

    }
}

