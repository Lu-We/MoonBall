using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerCamera : MonoBehaviour
{

    public float rotationSpeed;

    private void Update() {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
        
    }
}
