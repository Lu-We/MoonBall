using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isWalking = false;

    private void FixedUpdate() {
        isGrounded  = CheckGroundedState();
        isWalking   = CheckWalkingState();
    }


    
    private bool CheckGroundedState()
    {    
        RaycastHit hitinfo;
        Debug.DrawRay(transform.position + transform.up * 0.5f, -transform.up*1.5f, Color.blue);
        if(Physics.BoxCast(transform.position + transform.up * 0.25f, new Vector3(0.4f,0.1f,0.4f), -transform.up, out hitinfo, transform.rotation, 0.15f, groundLayer))
        {
            Debug.DrawRay(transform.position + transform.up * 0.5f, -transform.up*0.5f, Color.red);
            groundedRemember = CoyoteTime;
            return true;
        }
        else
        {
            groundedRemember -= Time.fixedDeltaTime;
            return false;
        }
    }

    private bool CheckWalkingState(){
        if( player.playerRb.velocity.magnitude > 0.1f ){
            return true;
        }else{
            return false;
        }
    }

    // Setter for isGrounded
    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
    }

    // Getter for isGrounded
    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    // Setter for isJumping
    public void SetIsJumping(bool value)
    {
        isJumping = value;
    }

    // Getter for isJumping
    public bool GetIsJumping()
    {
        return isJumping;
    }

    // Setter for isFalling
    public void SetIsFalling(bool value)
    {
        isFalling = value;
    }

    // Getter for isFalling
    public bool GetIsFalling()
    {
        return isFalling;
    }

    // Setter for isWalking
    public void SetIsWalking(bool value)
    {
        isWalking = value;
    }

    // Getter for isWalking
    public bool GetIsWalking()
    {
        return isWalking;
    }
}

