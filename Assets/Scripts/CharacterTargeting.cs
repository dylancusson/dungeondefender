using UnityEngine;

public class CharacterTargeting : MonoBehaviour
{
    [Header("Targeting Settings")]
    public float detectionRadius = 10f;
    public string targetTag = "Player"; // Tag of the target to detect

    [HideInInspector] public GameObject target; // Target GameObject to move towards

    private CharacterStats stats;
    private Character characterController;

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        characterController = GetComponent<Character>();
    }

    // Finds the closest target within the detection radius
    public void FindTarget()
    {
        if (target != null) return; // Already have a target

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                target = hitCollider.gameObject;
                Debug.Log("Target found: " + target.name);
                characterController.SetState(Character.State.moving);
                return;
            }
        }
    }

    // Clears the target and resets the state
    public void ClearTarget()
    {
        target = null;
        characterController.SetState(Character.State.idle);
        Debug.Log("Target cleared.");
    }

    // Checks if the target is within the character's attack range
    public bool IsTargetInAttackRange()
    {
        if (target == null || stats == null) return false;

        // Use the currentAttackRange from the Stats component
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance <= stats.currentAttackRange;
    }

    // Draw Gizmos for visualization
    private void OnDrawGizmosSelected()
    {
        if (stats == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.currentAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public Vector3 GetTargetPos()
    {
        if (target != null)
        {
            return target.transform.position;
        }
        else
        {
            return Vector3.zero;
        }

    }
}