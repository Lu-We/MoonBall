using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raquette : MonoBehaviour
{
    internal PlayerControls controls;

    private Vector3 basePos;
    private Vector2 moveInput;

    private int curveNr = 1;

    private void Start() {
        basePos = transform.localPosition;
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

    private void Update() {
        Debug.Log(moveInput);
         if(moveInput.y > 0.2f){
                transform.localPosition = basePos + transform.up;
                curveNr = 2;
            }
            else if(moveInput.y <= 0.2f && moveInput.y >= -0.2f){

                Vector3 offset = moveInput.x == 0f ? Vector3.zero : moveInput.x >= 0f ? -Vector3.right : Vector3.right;

                transform.localPosition = basePos + offset;
                curveNr = 1;
            }
            else if(moveInput.y < 0.2f ){
                transform.localPosition = basePos - transform.up;
                curveNr = 0;
            }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Ball")){
            Ball ballHit = other.GetComponent<Ball>();

            ballHit.SetCurve(curveNr);           
            
            ballHit.Accelerate(5f);
            ballHit.ChangeDirection();
        }
    }
}
