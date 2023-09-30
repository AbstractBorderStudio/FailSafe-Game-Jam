using UnityEngine;

public class testMovement : MonoBehaviour
{
    public float translationSpeed = 10f;

    void Update()
    {
        // Get the current position
        Vector3 currentPosition = transform.position;

        // Calculate the new position with z-axis translation
        Vector3 newPosition = currentPosition + new Vector3( translationSpeed * Time.deltaTime, 0f, 0f);

        // Assign the new position to the transform
        transform.position = newPosition;
    }
}
