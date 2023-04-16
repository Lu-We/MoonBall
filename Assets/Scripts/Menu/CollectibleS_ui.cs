using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleS_ui : MonoBehaviour
{

    [Header("Movement")]
    public float RotSpeed = 5;
    public float SinSpeed = 1;
    public float SinAmp = 1f;
    

    private Vector3 BasePosition;
    private float RandomOffset;

    private void Start() {
        BasePosition = transform.position;
        RandomOffset = Random.Range(-2*Mathf.PI, +2*Mathf.PI);    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * RotSpeed * Time.deltaTime);
        //transform.position = BasePosition + Vector3.up * Mathf.Sin(Time.time * SinSpeed + RandomOffset)  * SinAmp; 
    }
}
