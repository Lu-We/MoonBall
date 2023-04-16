using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1.0f; // Speed of floating
    public float floatHeight = 0.5f; // Height of floating
    public float rotationSpeed = 45.0f; // Speed of rotation
    public bool floatVertically = true; // Whether to float vertically or not

    private Vector3 originalPosition; // Original position of the object
    private Vector3 rotationAxis; // Random rotation axis

    void Start()
    {
        originalPosition = transform.position; // Store the original position of the object
        GenerateRandomRotationAxis(); // Generate initial random rotation axis
    }

    void Update()
    {
        // Calculate the new position based on the original position and time
        Vector3 newPosition = originalPosition + new Vector3(0.0f, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0.0f);

        if (floatVertically)
        {
            // Move the object to the new position
            transform.position = newPosition;
        }
        else
        {
            // Move the object only on the X and Z axes to create a floating effect on a plane
            transform.position = new Vector3(newPosition.x, originalPosition.y, newPosition.z);
        }

        // Rotate the object smoothly on the random rotation axis
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    void GenerateRandomRotationAxis()
    {
        // Generate a random rotation axis
        rotationAxis = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }
}