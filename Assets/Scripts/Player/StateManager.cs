using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    internal PlayerScript player;
    public LayerMask groundLayer;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isWalking = false;
    private bool isDashing = false;

    private float CoyoteTime = 0.2f;
    public float groundedRemember;
    public bool groundedLastFrame = false;

    Coroutine dashStateSet;

    private void Start() {
        player = GetComponent<PlayerScript>();
    }

    private void FixedUpdate() {

        groundedLastFrame = isGrounded;

        isGrounded  = CheckGroundedState(); 
        isWalking   = CheckWalkingState();  

    }


    
    private bool CheckGroundedState()
    {    
        RaycastHit hitinfo;
        Debug.DrawRay(transform.position + transform.up * 0.25f, -transform.up*0.25f, Color.blue);
        if(Physics.SphereCast(transform.position + transform.up * 0.25f, 0.1f , -transform.up, out hitinfo, 0.2f, groundLayer))
        {
            //SetIsJumping(false);
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

    public void TriggerDashState(float duration){     
        dashStateSet = StartCoroutine(DashStateSet(duration));
    }
    
    public void ExitDashState(){
        if(dashStateSet != null)
            StopCoroutine(dashStateSet);
        isDashing = false;
        player.movementManager.StopDash();  
    }

    public IEnumerator DashStateSet(float timeInSec){
        isDashing = true;
        yield return new WaitForSeconds(timeInSec);
        ExitDashState();  
    }

    // Setter for isGrounded
    public void SetIsGrounded(bool value)
    {
        groundedLastFrame = value;
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

    // Setter for isDashing
    public void SetIsDashing(bool value)
    {
        isDashing = value;
    }

    // Getter for isDashing
    public bool GetIsDashing()
    {
        return isDashing;
    }
}

