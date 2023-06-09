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

    public GameObject raquetteprefab;
        //Jump Buffers
    private float jumpPressedRememberTime = 0.2f;
    private float jumpPressedRemember;
    public  float dashCooldown = 1f;
    private float nextDashTime = 0f;
    private float dashPressedRememberTime = 0.2f;
    private float dashPressedRemember;

    // attack
    private float attackUpPressedRememberTime = 0.2f;
    private float attackUpPressedRemember;
    private float attackMidPressedRememberTime = 0.2f;
    private float attackMidPressedRemember;
    private float attackDownPressedRememberTime = 0.2f;
    private float attackDownPressedRemember;
    public  float attackCooldown = .5f;
    private float nextAttackTime = 0f;

    private bool isReversed = false;

    private void Start() {
        player = GetComponent<PlayerScript>();
        if(player.GetPlayerIndex() == 1 ){
            isReversed = true;
        }

    }


    public PlayerControls CreateControls()
    {
        controls = new PlayerControls(); 

        controls.Player.Move.performed += ctx => OnMove(ctx);
        controls.Player.Move.canceled  += ctx => OnStop(ctx);

        controls.Player.Jump.performed += ctx => JumpPressed();
        controls.Player.Jump.canceled  += ctx => JumpCanceled();

        controls.Player.Dash.performed += ctx => DashPressed();   

        controls.Player.AttackUp.performed += ctx => AttackUpPressed();
        //controls.Player.AttackUp.canceled += ctx => AttackUpcanceled();

        controls.Player.AttackMid.performed += ctx => AttackMidPressed();
       // controls.Player.AttackMid.canceled += ctx => AttackMidcanceled();   

        controls.Player.AttackDown.performed += ctx => AttackDownPressed();
        //controls.Player.AttackDown.canceled += ctx => AttackDowncanceled();   

        return controls;
    }

    private void OnEnable() {
        if(controls != null){
            controls.Player.Enable();
        }
    }

    private void OnDisable() {
        if(controls != null){
            controls.Player.Disable();
        }
    }
    

    void FixedUpdate()
    {   
        CheckJump();
        CheckDash();
        CheckAttackMid();
        CheckAttackDown();
        CheckAttackUp();

        if(moveInput.y < -0.2f){
            //Debug.Log("Crouch");
            player.stateManager.SetIsCrouching(true);
        }else{
            player.stateManager.SetIsCrouching(false);
        }
    }

    private void OnMove(CallbackContext ctx){
        if(isReversed){
            moveInput.y = ctx.ReadValue<Vector2>().y;
            moveInput.x = -ctx.ReadValue<Vector2>().x;
        }
        else{
            moveInput = ctx.ReadValue<Vector2>();
        }
    }

    private void OnStop(CallbackContext ctx){
        moveInput = moveInput = Vector2.zero;
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

    private void AttackUpPressed(){
        attackUpPressedRemember = attackUpPressedRememberTime;
    }

    private void CheckAttackUp(){
        attackUpPressedRemember -= Time.fixedDeltaTime;

        if( attackUpPressedRemember > 0f && Time.time > nextAttackTime)
        {
            player.animator.SetBool("AttackUp",true);
            player.audioManager.PlayAttackUpSFX();
            GameObject hitbox;
            Vector3 offset ;
            int curve;
            offset = new Vector3(0, 3, 0);
            curve = 2;            

            hitbox = Instantiate(raquetteprefab,player.transform.position, Quaternion.identity,player.transform);
            hitbox.GetComponent<raquette>().InitRaquette(offset,8,.15f,curve);

            nextAttackTime = Time.time + attackCooldown;
        }        
    }

    private void AttackMidPressed(){
        attackMidPressedRemember = attackMidPressedRememberTime;
    }

    private void CheckAttackMid(){

        attackMidPressedRemember -= Time.fixedDeltaTime;

        if( attackMidPressedRemember > 0f && Time.time > nextAttackTime ){
            
            player.animator.SetBool("AttackMid",true);
            player.audioManager.PlayAttackMidSFX();
            GameObject hitbox;
            Vector3 offset = new Vector3(0, 2, 0);
            hitbox = Instantiate(raquetteprefab,player.transform.position, Quaternion.identity,player.transform);
            hitbox.GetComponent<raquette>().InitRaquette(offset,8,0.15f,1);

            nextAttackTime = Time.time + attackCooldown;
        }       
    }

    private void AttackDownPressed(){
        attackDownPressedRemember = attackDownPressedRememberTime;
    }

    private void CheckAttackDown(){

        attackDownPressedRemember -= Time.fixedDeltaTime;

        if( attackDownPressedRemember > 0f && Time.time > nextAttackTime){
            player.animator.SetBool("AttackDown",true);
            player.audioManager.PlayAttackDownSFX();
            GameObject hitbox;
            Vector3 offset;
            int curve;

            offset = new Vector3(0, 1, 0);
            curve = 0;
    
            hitbox = Instantiate(raquetteprefab,player.transform.position, Quaternion.identity,player.transform);
            hitbox.GetComponent<raquette>().InitRaquette(offset,8,.15f,curve);

            nextAttackTime = Time.time + attackCooldown;
        }        
    }

    private void LateUpdate() {
        player.animator.SetBool("AttackMid",false);
        player.animator.SetBool("AttackUp",false);
        player.animator.SetBool("AttackDown",false);
    }


}
