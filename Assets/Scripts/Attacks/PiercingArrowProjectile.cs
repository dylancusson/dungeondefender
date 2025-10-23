using System.Collections.Generic;
using UnityEngine;

public class PiercingArrowProjectile : MonoBehaviour
{
    public float maxDistance = 20f;

    // --- Piercing Logic Fields ---
    public int pierceCount = 1; // How many targets it can pierce
    private HashSet<GameObject> targetsHit = new HashSet<GameObject>(); // Tracks unique enemies hit
    // -----------------------------

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private int damage;
    private string targetTag;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Destroy the arrow if it goes max distance
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    // This method is called when the projectile's Trigger Collider hits another object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the projectile hit a target
        if (other.CompareTag(targetTag))
        {
            Debug.Log(gameObject.name + " collided with " + other.gameObject.name);
            // 2. Check if this target has already been hit by this specific arrow
            if (!targetsHit.Contains(other.gameObject))
            {
                // 3. Add the target to the hit list
                targetsHit.Add(other.gameObject);

                // 4. Deal damage
                CharacterHealth enemyHealth = other.GetComponent<CharacterHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log(gameObject.name + " hit " + other.gameObject.name + " for " + damage + " damage.");
                }

                // 5. Reduce pierce count
                pierceCount--;

                // 6. Check if the arrow should be destroyed
                if (pierceCount <= 0)
                {
                    Destroy(gameObject);
                }
            }
            // If the target has been hit, we do nothing and let the arrow pass through (pierce)
        }
    }

    // --- Public Setter Methods ---
    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetTargetTag(string tag)
    {
        targetTag = tag;
    }
}