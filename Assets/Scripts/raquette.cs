using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raquette : MonoBehaviour
{
    internal PlayerControls controls;

    private Vector3 basePos;
    private SphereCollider hitbox;

    private int curveNr = 1;

    private void Start() {
        basePos = transform.localPosition;
        hitbox = GetComponent<SphereCollider>();        
    }

    public void InitRaquette(Vector3 offset,float radius, float duration, int curve){
        Destroy(gameObject,duration);
        transform.localPosition += offset;
        transform.localScale = new Vector3(radius,radius,radius);       
        curveNr = curve;        
    }    

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Ball")){
            Ball ballHit = other.GetComponent<Ball>();
            ballHit.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ballHit.SetCurve(curveNr);           
            
            ballHit.Accelerate(5f);
            ballHit.ChangeDirection();
        }
    }
}
