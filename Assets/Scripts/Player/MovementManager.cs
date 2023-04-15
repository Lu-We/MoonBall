using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    internal PlayerScript player;
    public LayerMask groundLayer;

    public float moveSpeed = 5f;


    public float rotationSpeed = 50f;

    public float jumpForce = 10f;
    public float jumpFalloff = 2.5f;
    public float gravity = -50f;

    

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
        Vector3 playerVel = Vector3.zero;
        player.playerRb.velocity = Vector3.zero;
        
        playerVel += MovePlayer(player.inputManager.GetMoveInput()); 
    

        // if(lastFrameVelocity.magnitude != 0 && (moveDirection.normalized.x != 0 || moveDirection.normalized.z != 0) ){
        //         Quaternion rot = Quaternion.LookRotation(player);
        //         player.playerRb.transform.rotation = rot;
        // }

        //player.playerRb.velocity = playerVel;  
        lastFrameGrounded = player.stateManager.GetIsGrounded();
    
    }

    public Vector3 MovePlayer(Vector2 moveInput){
        
        Vector3 playerVel = Vector3.zero; 
        Vector3 corrHor = moveInput.x * Camera.main.transform.right;
        Vector3 corrVer = moveInput.y * Camera.main.transform.forward;
        Vector3 combinedInput = corrHor + corrVer;
        moveDirection = new Vector3(combinedInput.x,0f,0f * combinedInput.y).normalized;
       
        // Calculate player rotation based on sphere's normal
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + transform.up * 0.25f, -transform.up, out hitInfo, Mathf.Infinity))
        {
            Vector3 normal  = hitInfo.normal;

            Quaternion moveDirectionRotation = Quaternion.LookRotation(moveDirection);
            transform.GetChild(0).localRotation = moveDirectionRotation;

            Quaternion targetRotation   = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            moveDirection   = transform.TransformDirection(moveDirection);


             
            
            
           

            

            

            //movement
            if(player.stateManager.GetIsGrounded()){
                //acceleration & deceleration
                playerVel   = Vector3.Lerp(lastFrameVelocity, moveDirection * moveSpeed, 15*Time.fixedDeltaTime );  
                gravityVel  = Vector3.zero;                 
            }
            else{
                playerVel   = Vector3.Lerp(lastFrameVelocity, moveDirection * moveSpeed, 7*Time.fixedDeltaTime );

                gravityVel = lastFrameGravity.magnitude * -normal;                
                gravityVel += (normal * gravity * (jumpFalloff-1) * Time.fixedDeltaTime);

            }
            



            lastFrameVelocity = playerVel;
            lastFrameGravity  = gravityVel;  
        
            // Apply the rotated velocity back to the rigidbody
            player.playerRb.velocity = playerVel + gravityVel;


        }
  
        

        return playerVel;
    }

}
