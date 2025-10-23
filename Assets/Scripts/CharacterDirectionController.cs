using UnityEngine;
using UnityEngine.AI;

public class CharacterDirectionController : MonoBehaviour
{
    private NavMeshAgent agent;

    // A reference to the GameObject whose scale you want to flip.
    // This is often a child object containing the sprite/mesh.
    // If you flip the root 'transform', it flips the agent too!
    public Transform spriteToFlip;

    // Optional: Use this to control how fast the character must be moving sideways
    private const float HorizontalMoveThreshold = 0.05f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
            enabled = false;
        }

        if (spriteToFlip == null)
        {
            // Fallback: Use the main transform if no specific child is assigned
            spriteToFlip = transform;
            Debug.LogWarning("spriteToFlip not assigned. Using root transform for flipping.");
        }
    }

    void Update()
    {
        // 1. Only check if the agent is moving
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            // 2. Convert world velocity into a vector relative to the character's forward
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

            // 3. Determine direction based on the horizontal (X) component of local velocity

            if (localVelocity.x > HorizontalMoveThreshold)
            {
                // Moving Right: Set the sprite to face right (e.g., scale X = 1 or rotation Y = 0)
                SetFacingRight();
            }
            else if (localVelocity.x < -HorizontalMoveThreshold)
            {
                // Moving Left: Set the sprite to face left (e.g., scale X = -1 or rotation Y = 180)
                SetFacingLeft();
            }
        }
    }

    private void SetFacingRight()
    {
        // Option A: Flip by changing the Y-rotation (3D characters)
        // spriteToFlip.localRotation = Quaternion.Euler(0, 0, 0); 

        // Option B: Flip by changing the X-scale (2D sprites)
        // Note: We only flip the X-scale, keeping Y and Z scales intact.
        Vector3 newScale = spriteToFlip.localScale;
        newScale.x = Mathf.Abs(newScale.x); // Assumes right-facing is positive scale
        spriteToFlip.localScale = newScale;
    }

    private void SetFacingLeft()
    {
        // Option A: Flip by changing the Y-rotation (3D characters)
        // spriteToFlip.localRotation = Quaternion.Euler(0, 180, 0);

        // Option B: Flip by changing the X-scale (2D sprites)
        // Note: We multiply by -1 to flip the scale.
        Vector3 newScale = spriteToFlip.localScale;
        newScale.x = -Mathf.Abs(newScale.x); // Assumes left-facing is negative scale
        spriteToFlip.localScale = newScale;
    }
}