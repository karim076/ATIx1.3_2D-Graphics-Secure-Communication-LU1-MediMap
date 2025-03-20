using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Assign your avatar (player) in the Inspector
    public float smoothSpeed = 5f; // Adjust for smooth movement

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Target (avatar) not assigned to CameraFollow script!");
            return;
        }

        // Get the target position (keep the same Z position)
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Smoothly move the camera towards the target
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
