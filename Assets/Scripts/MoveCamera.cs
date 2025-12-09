using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;

    [Header("Level Boundaries")]
    // Define the positive limits. The script assumes the level is centered at 0,0
    // so it limits between -xLimit and +xLimit.
    [SerializeField] private float xLimit = 20f;
    [SerializeField] private float yLimit = 15f;

    void Update()
    {
        // 1. Reset movement vector every frame
        Vector3 movement = Vector3.zero;

        // 2. Build the movement vector based on Input
        // (Using GetKey allows for smooth continuous checking)
        if (Input.GetKey(KeyCode.W)) movement.y += 1;
        if (Input.GetKey(KeyCode.S)) movement.y -= 1;
        if (Input.GetKey(KeyCode.A)) movement.x -= 1;
        if (Input.GetKey(KeyCode.D)) movement.x += 1;

        // 3. Normalize to prevent faster diagonal movement
        movement = movement.normalized;

        // 4. Apply movement to current position
        // We move the camera freely first...
        Vector3 targetPosition = transform.position + (movement * speed * Time.deltaTime);

        // 5. Clamp the result to keep it within bounds
        // ...then we snap it back if it went too far.
        float clampedX = Mathf.Clamp(targetPosition.x, -xLimit, xLimit);
        float clampedY = Mathf.Clamp(targetPosition.y, -yLimit, yLimit);

        // 6. Update the actual transform
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
    // Draws a red box in the Scene view to show the camera limits
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Draw a wire cube representing the bounds
        // Size is limit * 2 because limits are half-extents (from center to edge)
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(xLimit * 2, yLimit * 2, 0));
    }
}