using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    internal PlayerScript player;

    internal PlayerControls controls;
    public Vector2 moveInput;

        //Jump Buffers
    private float jumpPressedRememberTime = 0.2f;
    private float jumpPressedRemember;

    private void Start() {
        player = GetComponent<PlayerScript>();
    }


    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => JumpPressed();
        controls.Player.Jump.canceled  += ctx => JumpCanceled();
    }

    private void OnEnable() {
       controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {   
        CheckJump();
    }

    private void JumpPressed(){
        jumpPressedRemember = jumpPressedRememberTime;
    }

    private void JumpCanceled(){
        if(player.stateManager.GetIsJumping())
            player.movementManager.CutJump(false);
    }

    private void CheckJump(){
        jumpPressedRemember -= Time.fixedDeltaTime;

        //Check Jumping Intent and if player was (grounded + Coyotte time)
        if(player.stateManager.groundedRemember > 0 && (jumpPressedRemember > 0))
        {
            jumpPressedRemember = 0;
            player.stateManager.groundedRemember = 0; 

            player.stateManager.SetIsJumping(true);
            player.stateManager.SetIsGrounded(false);
            player.movementManager.PerformJump();       
        }
    }

    public virtual Vector2 GetMoveInput(){
        return moveInput;
    }


}
