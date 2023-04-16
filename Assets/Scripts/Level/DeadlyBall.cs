using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyBall : MonoBehaviour
{
    public Transform moon;

    public float ballSpeed  = 80f;
    public float minSpeed   = 20f;
    public float medSpeed   = 120f;
    public float maxSpeed   = 80f;
    
    public int curveNr = 0;

    public float curve1 = 27f;
    public float curve2 = 30f;
    public float curve3 = 33f;
    
    public float lerpSpeed = 1f;

    private float offset = -27f;
    private float damageMultiply = 5f;

    private Renderer myrenderer;
    private Rigidbody myRb;

    // Start is called before the first frame update
    void Start()
    {
        myrenderer = GetComponent<Renderer>();
        myRb = transform.GetComponent<Rigidbody>();
        offset = -(moon.position - transform.position).magnitude;
        Destroy(gameObject, 10f);
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

        Debug.Log(offset);
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
        ballSpeed *= 1f;
        ballSpeed = Mathf.Clamp(ballSpeed,minSpeed, maxSpeed);
    }

    public void Deccelerate(float amount){
         ballSpeed /= 1f;
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
