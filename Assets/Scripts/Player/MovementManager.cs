using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    internal PlayerScript player;

    public float moveSpeed = 5f;

    private void Start() {
        player = GetComponent<PlayerScript>;
    }

    private void FixedUpdate()
    {
        // Detect ground surface
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            // Calculate surface normal
            Vector3 surfaceNormal = hit.normal;

            // Orient player character
            transform.rotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;

            // Apply movement
            float horizontal = player.inputManager.moveInput.x;
            float vertical = player.inputManager.moveInput.y;
            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
            Vector3 moveDirectionWorld = transform.TransformDirection(moveDirection);
            rb.velocity = moveDirectionWorld * moveSpeed;
        }
    }

}
