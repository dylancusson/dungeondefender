using UnityEngine;

public class ArrowBasicAttack : BasicAttack
{
    public GameObject arrowPrefab;
    public float arrowSpeed = 20f;

    private CharacterStats stats;

    [SerializeField] private AudioClip SoundEffect;

    public void Awake()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab is not assigned.");
        }
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned.");
        }
        stats = GetComponent<CharacterStats>();
        damage = stats.currentAttackDmg;
    }

    public void FixedUpdate()
    {
        
    }
    // Method to instantiate and launch an arrow towards a target position
    public override void Attack(Vector3 targetPosition)
    {
        SoundFXManager.Instance.playSoundFXClip(SoundEffect, transform, 1f);
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<ArrowProjectile>().SetTargetPosition(targetPosition);
        Vector3 direction = (targetPosition - transform.position).normalized;
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * arrowSpeed;
        RotateArrow(arrow.transform, new Vector2(direction.x, direction.y));
    }

    private void RotateArrow(Transform arrowTransform, Vector2 direction)
    {
        // Use Atan2 to get the angle from the vector components
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation on the Z-axis
        // You might need to adjust 'angle' by +90 or -90 degrees depending 
        // on which way your arrow sprite is pointing by default.
        arrowTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
