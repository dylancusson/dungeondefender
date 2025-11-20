using System.Collections;
using TMPro;
using UnityEngine;

public class PiercingArrowSpecial : SpecialAttack
{
    public GameObject arrowPrefab;
    public float arrowSpeed = 20f;
    public int shotsToFire = 3;
    public float delayBetweenShots = 0.2f;
    private CharacterStats stats;
    private CharacterTargeting targeting;

    [SerializeField] private AudioClip SoundEffectOne;
    [SerializeField] private AudioClip SoundEffectTwo;

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
        targeting = GetComponent<CharacterTargeting>();
    }

    public void FixedUpdate()
    {

    }
    // Method to instantiate and launch an arrow towards a target position
    public override void SpecialATK()
    {
        SoundFXManager.Instance.playSoundFXClip(SoundEffectOne, transform, 1f);
        StartCoroutine(FireMultipleArrows());
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

    private void FireArrowAtTarget(Vector3 targetPos, int dmg, string targetTag)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<PiercingArrowProjectile>().SetTargetPosition(targetPos);
        arrow.GetComponent<PiercingArrowProjectile>().SetDamage(dmg);
        arrow.GetComponent<PiercingArrowProjectile>().SetTargetTag(targetTag);

        Vector3 direction = (targetPos - transform.position).normalized;
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * arrowSpeed;
        RotateArrow(arrow.transform, new Vector2(direction.x, direction.y));
    }

    private IEnumerator FireMultipleArrows()
    {
        int shotsFired = 0;
        int damage = stats.currentAttackDmg / 2;
        string targetTag = targeting.targetTag;
        Vector3 targetPosition = targeting.GetTargetPos();
        while (shotsFired < shotsToFire)
        {
            SoundFXManager.Instance.playSoundFXClip(SoundEffectTwo, transform, 1f);
            FireArrowAtTarget(targetPosition, damage, targetTag);
            shotsFired++;

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }
}
