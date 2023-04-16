using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{   
    private PlayerInput playerInput;
    private PlayerScript player;
   
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<PlayerScript>();
        var index = playerInput.playerIndex;
        
        player = players.FirstOrDefault(p => p.GetPlayerIndex() == index);
        playerInput.actions = player.inputManager.CreateControls().asset;
    }

}
