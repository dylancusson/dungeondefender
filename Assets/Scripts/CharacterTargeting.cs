using UnityEngine;
using System.Linq; // Added for sorting logic if needed, though manual is faster

public class CharacterTargeting : MonoBehaviour
{
    [Header("Targeting Settings")]
    public float detectionRadius = 10f;
    public string enemyTag = "Enemy"; // The tag of the enemies
    public string nexusTag = "Nexus"; // The tag of the structure at the end of the zone

    [HideInInspector] public GameObject target;
    private CharacterStats stats;
    private Character characterController;
    private GameObject cachedNexus; // Store nexus so we don't Find it every frame

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        characterController = GetComponent<Character>();

        // Cache the nexus to save performance
        GameObject nexusObj = GameObject.FindGameObjectWithTag(nexusTag);
        if (nexusObj != null) cachedNexus = nexusObj;
        else Debug.LogWarning($"No GameObject with tag '{nexusTag}' found in scene!");
    }

    // Logic: Find closest enemy. If none, return Nexus.
    public void FindBestTarget()
    {
        GameObject bestCandidate = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        // 1. Scan for Enemies
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentPos, detectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(enemyTag))
            {
                // Check distance to find the closest one
                Vector3 directionToTarget = hitCollider.transform.position - currentPos;
                float dSqrToTarget = directionToTarget.sqrMagnitude; // sqrMagnitude is faster than Distance

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestCandidate = hitCollider.gameObject;
                }
            }
        }

        // 2. Logic Evaluation
        if (bestCandidate != null)
        {
            // We found an enemy
            if (target != bestCandidate)
            {
                target = bestCandidate;
                Debug.Log("Switched target to enemy: " + target.name);
                characterController.SetState(Character.State.moving);
            }
        }
        else
        {
            // No enemies in range. Do we have a Nexus?
            if (cachedNexus != null)
            {
                // Only switch if we aren't already targeting it and are an enemy
                if (target != cachedNexus && this.tag == "Enemy")
                {
                    target = cachedNexus;
                    // Debug.Log("No enemies. Moving to Nexus.");
                    characterController.SetState(Character.State.moving);
                }
            }
            else
            {
                // No enemies AND no Nexus found
                target = null;
                characterController.SetState(Character.State.idle);
            }
        }
    }

    public bool IsTargetInAttackRange()
    {
        if (target == null || stats == null) return false;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance <= stats.currentAttackRange;
    }

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