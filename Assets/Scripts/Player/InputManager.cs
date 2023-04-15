using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    internal PlayerScript player;

    internal PlayerControls controls;
    public Vector2 moveInput;

    private void Start() {
        player = GetComponent<PlayerScript>();
    }


    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable() {
       controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {   

    }

    public virtual Vector2 GetMoveInput(){
        return moveInput;
    }


}
