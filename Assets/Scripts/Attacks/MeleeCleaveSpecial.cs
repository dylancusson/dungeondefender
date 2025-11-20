using UnityEngine;

public class MeleeCleaveSpecial : SpecialAttack
{
    private CharacterStats stats;
    private CharacterTargeting targeting;
    private CharacterHealth myHealth;

    [SerializeField] private AudioClip SoundEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Initialization code can go here if needed
        stats = GetComponent<CharacterStats>();
        targeting = GetComponent<CharacterTargeting>();
        myHealth = GetComponent<CharacterHealth>();
    }

    public override void SpecialATK()
    {

        float cleaveRange = (4f * stats.currentAttackRange); // units
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, cleaveRange);
        foreach (var hitCollider in hitColliders)
        {
            // Check if the collider belongs to an enemy
            if (hitCollider.CompareTag(targeting.targetTag))
            {
                CharacterHealth enemyHealth = hitCollider.GetComponent<CharacterHealth>();
                if (enemyHealth != null)
                {
                    SoundFXManager.Instance.playSoundFXClip(SoundEffect, transform, 1f);
                    enemyHealth.TakeDamage(2 * stats.currentAttackDmg);
                    myHealth.TakeDamage(-1 * stats.currentAttackDmg); // Heal self for half the damage dealt

                }
            }
        }
    }
}
