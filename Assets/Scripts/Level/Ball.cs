using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform moon;

    public Material medSpeedMat;
    public Material maxSpeedMat;
    private Material baseMat;

    public float ballSpeed  = 50f;
    public float minSpeed   = 20f;
    public float medSpeed   = 120f;
    public float maxSpeed   = 200f;
    
    public int curveNr = 0;

    public float curve1 = 27f;
    public float curve2 = 30f;
    public float curve3 = 33f;
    
    public float lerpSpeed = 5f;

    private float offset = -27f;
    private float damageMultiply = 1f;

    private Renderer myrenderer;
    private Rigidbody myRb;

    // Start is called before the first frame update
    void Start()
    {
        myrenderer = GetComponent<Renderer>();
        baseMat = myrenderer.material;
        myRb = transform.GetComponent<Rigidbody>();
        offset = -(moon.position - transform.position).magnitude;
    }

    private void Update() {
        //ResetZRot();
        CheckMaxSpeedReach();
    }

    private void ResetZRot(){
        // Get the current local rotation
        Quaternion currentRotation = transform.localRotation;

        // Set the Z-axis rotation to 0
        currentRotation = Quaternion.Euler(currentRotation.eulerAngles.x , 90f, currentRotation.eulerAngles.z);

        // Set the updated local rotation back to the game object
        transform.localRotation = currentRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 gravityUp = (moon.position - transform.position).normalized;
        Vector3 bodyUp = transform.up;

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp,gravityUp) * transform.rotation;
        transform.rotation = targetRotation;

        switch(curveNr){
            case 0:
                offset = Mathf.Lerp(offset, -curve1, lerpSpeed * Time.fixedDeltaTime);
                break;
            case 1:
                offset = Mathf.Lerp(offset, -curve2, lerpSpeed * Time.fixedDeltaTime);
                break;
            case 2:
                offset = Mathf.Lerp(offset, -curve3, lerpSpeed * Time.fixedDeltaTime);
                break;
        }

      
        transform.position = gravityUp * offset;

        
        myRb.velocity = transform.forward * ballSpeed;
    }

    public void SetMoon(Transform m){
        moon = m;
    }

    public void SetCurve(int number){
        curveNr = number;
    }

    public void SetDamageMultiply(float factor){
        damageMultiply = factor;
    }

    public void Accelerate(float amount){
        ballSpeed *= 1.05f;
        ballSpeed = Mathf.Clamp(ballSpeed,minSpeed, maxSpeed);
    }

    public void Deccelerate(float amount){
         ballSpeed /= 1.1f;
         ballSpeed = Mathf.Clamp(ballSpeed,minSpeed, maxSpeed);
    }

    public void SetMaxSpeed(){
        ballSpeed = maxSpeed;
    }

    public void SetSpeed(float speed){
        ballSpeed = speed;
    }

    public void ChangeDirection(){        
            transform.Rotate(transform.up, 180, Space.Self);
    }

    public void CheckMaxSpeedReach(){

        if(ballSpeed >= maxSpeed){
            myrenderer.material = maxSpeedMat;
        }
        else if(ballSpeed >= medSpeed){
            myrenderer.material = medSpeedMat;

        }else if(ballSpeed < medSpeed){
             myrenderer.material = baseMat;
        }
        
    }


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Hurtbox")){
            PlayerScript player = other.GetComponentInParent<PlayerScript>();
            if(player == null)
                return;
            player.playerHealth.hitNormal = myRb.velocity;
            player.playerHealth.InflictDamage( ballSpeed / 10f * damageMultiply);
            //Debug.Log(player.playerHealth.GetHealth());
        }
    }
    
}
