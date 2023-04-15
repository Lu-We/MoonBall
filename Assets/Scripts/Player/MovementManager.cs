using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    internal PlayerScript player;
    public LayerMask groundLayer;

    public float moveSpeed = 5f;
    Vector3 playerVel = Vector3.zero; 


    public float rotationSpeed = 50f;

    public float dashForce = 100f;
    public float dashTime = 10f;

   /********** JUMP **********/
    //Jump parameters
    public float jumpForce = 10f;  
    public float jumpFalloff = 2.5f;
    public float cutJumpHeight=0.5f;

    /**************************/

    public float gravity = -9.8f; // earth gravity
    private Vector3 gravityNormal;
    

    private bool lastFrameGrounded = false;

    private Vector3 lastFrameVelocity   = Vector3.zero;
    private Vector3 lastFrameGravity    = Vector3.zero;
    private Vector3 moveDirection       = Vector3.zero;
    private Vector3 gravityVel          = Vector3.zero;

    private void Start() {
        player = GetComponent<PlayerScript>();
    }

    private void FixedUpdate()
    {  
        playerVel = Vector3.zero;
        player.playerRb.velocity = Vector3.zero;
        
        MovePlayer(player.inputManager.GetMoveInput()); 
        CheckJumpCollider();


        if(Vector3.Dot(gravityVel.normalized , gravityNormal.normalized) < 0f && player.stateManager.GetIsJumping() ){
            player.stateManager.SetIsJumping(false);
        }





        player.playerRb.velocity = playerVel + gravityVel;

        lastFrameGrounded = player.stateManager.GetIsGrounded();                    
        lastFrameVelocity = playerVel;
        lastFrameGravity  = gravityVel;
    
    }

    private void MovePlayer(Vector2 moveInput){
        
        
        Vector3 corrHor = moveInput.x * Camera.main.transform.right;
        Vector3 corrVer = moveInput.y * Camera.main.transform.forward;
        Vector3 combinedInput = corrHor + corrVer;

        if( combinedInput.x < 0.2f && combinedInput.x > -0.2f){
            combinedInput.x = 0f;
        }

        moveDirection = new Vector3(combinedInput.x,0f,0f * combinedInput.y).normalized;

       
        // Calculate player rotation based on sphere's gravityNormal
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + transform.up * 0.25f, -transform.up, out hitInfo, Mathf.Infinity))
        {
            gravityNormal  = hitInfo.normal;

            //rotate body yaw to match direction
            if(moveDirection.magnitude > 0f){
                Quaternion moveDirectionRotation = Quaternion.LookRotation(moveDirection);
                transform.GetChild(0).localRotation = moveDirectionRotation;
            }

            //rotate all to match surface
            Quaternion targetRotation   = Quaternion.FromToRotation(transform.up, gravityNormal) * transform.rotation;
            transform.rotation =  targetRotation;//Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            moveDirection   = transform.TransformDirection(moveDirection);

            if(player.stateManager.GetIsDashing()){
                if( moveDirection.magnitude == 0){
                    playerVel = lastFrameVelocity.magnitude * transform.GetChild(0).forward;
                }else{
                    playerVel = lastFrameVelocity.magnitude * moveDirection;
                }
            }else{
                //movement
                if(player.stateManager.GetIsGrounded()){
                    //acceleration & deceleration
                    playerVel = Vector3.Lerp(lastFrameVelocity.magnitude * moveDirection, moveDirection * moveSpeed, 15*Time.fixedDeltaTime );
                    
                    if(!player.stateManager.GetIsJumping()){
                        gravityVel = gravityNormal * -0.1f;
                        lastFrameGravity = gravityVel;
                    }
                                
                }
                else{
                    playerVel   = Vector3.Lerp(lastFrameVelocity.magnitude * moveDirection, moveDirection * moveSpeed, 3*Time.fixedDeltaTime );

                    gravityVel = lastFrameGravity.magnitude * gravityNormal * Mathf.Sign(Vector3.Dot(gravityVel.normalized,gravityNormal.normalized)) ;                
                    gravityVel += (gravityNormal * gravity * (jumpFalloff-1) * Time.fixedDeltaTime) ;
                }         
            }
                
        }
    }

    private void CheckJumpCollider()
    {
        //Cut jump when release or when hitting something
        RaycastHit hitinfo;
        if( player.stateManager.GetIsJumping() && ( Physics.BoxCast(transform.position + transform.up * transform.localScale.y, new Vector3(0.35f,0.1f,0.35f), Vector3.up, out hitinfo, transform.rotation, 0.15f, groundLayer)))
        {
            CutJump(true);
        }        
    }

    public void PerformJump(){
        Debug.Log("Jump");
        
        gravityVel = gravityNormal * jumpForce;
        Debug.Log(gravityVel.magnitude);
        //playerVel += gravityNormal * jumpForce; 
        lastFrameVelocity = playerVel;
        lastFrameGravity  = gravityVel;   
        player.playerRb.velocity = playerVel + gravityVel; 
    }

    public void CutJump(bool ColliderHit){

        if( Vector3.Dot(gravityVel.normalized , gravityNormal.normalized) > 0f)
        {           
            player.stateManager.SetIsJumping(false);
            
            Debug.Log("Cut Jump");
            if( ColliderHit){
                gravityVel = gravityNormal * -0.1f;                             
            }
            else{
                gravityVel = gravityVel * cutJumpHeight; 
            }
            lastFrameGravity  = gravityVel; 
        }
        player.playerRb.velocity = playerVel + gravityVel;
    }

    public void PerformDash(){
        Debug.Log("Dash");

        CutJump(false);

        player.stateManager.TriggerDashState(dashTime);
        playerVel = transform.forward * dashForce;
        lastFrameVelocity = playerVel;   

        player.playerRb.velocity = playerVel; 
    }

    public void StopDash(){

        playerVel = moveDirection * moveSpeed;
        lastFrameVelocity = playerVel;   

        player.playerRb.velocity = playerVel; 
    }

    
}
