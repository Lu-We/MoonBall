using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    internal PlayerScript player;

    internal PlayerControls controls;
    public Vector2 moveInput;

        //Jump Buffers
    private float jumpPressedRememberTime = 0.2f;
    private float jumpPressedRemember;
    public  float dashCooldown = 1f;
    private float nextDashTime = 0f;
    private float dashPressedRememberTime = 0.2f;
    private float dashPressedRemember;

    private void Start() {
        player = GetComponent<PlayerScript>();
    }


    public PlayerControls CreateControls()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => JumpPressed();
        controls.Player.Jump.canceled  += ctx => JumpCanceled();

        controls.Player.Dash.performed += ctx => DashPressed();   

        controls.Player.Attack.performed += ctx => AttackPressed();
        controls.Player.Attack.canceled += ctx => Attackcanceled();   

        return controls;
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
        CheckDash();

        if(moveInput.y < -0.2f){
            Debug.Log("Crouch");
            player.stateManager.SetIsCrouching(true);
        }else{
            player.stateManager.SetIsCrouching(false);
        }
    }

    private void DashPressed(){
        dashPressedRemember = dashPressedRememberTime;
    }

    private void CheckDash(){
        dashPressedRemember -= Time.fixedDeltaTime;
        //Check Jumping Intent and if player was (grounded + Coyotte time)
        if(dashPressedRemember > 0)
        {
            if(Time.time > nextDashTime)
            {
                dashPressedRemember = 0;
                nextDashTime = Time.time + dashCooldown;
                player.stateManager.SetIsDashing(true);
                player.movementManager.PerformDash();       
            }else{
                Debug.Log("Can't Dash : Cooldown = " + (nextDashTime - Time.time) );
            }
    }
        
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position+transform.up*2, 8f);

    }

    private void AttackPressed(){

        Collider[] hitColliders = Physics.OverlapSphere(transform.position+transform.up*2, 8f);
        foreach(var other in hitColliders){
            if(other.CompareTag("Ball")){
                Ball ballHit = other.GetComponent<Ball>();
                ballHit.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ballHit.SetCurve(2);           
                
                ballHit.Accelerate(5f);
                ballHit.ChangeDirection();
            }
        }

        player.GetComponent<ParticleSystem>().Play();
    }

    private void Attackcanceled(){
        player.raquette.SetActive(false);
    }

}
