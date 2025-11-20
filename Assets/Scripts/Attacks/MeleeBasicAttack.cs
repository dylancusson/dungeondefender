using UnityEngine;

public class MeleeBasicAttack : BasicAttack
{

    [SerializeField] private AudioClip SoundEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Attack(Vector3 targetPosition)
    {
        SoundFXManager.Instance.playSoundFXClip(SoundEffect, transform, 1f);
        // Calculate direction to target
        Vector3 direction = (targetPosition - spawnPoint.position).normalized;
        // For melee attack, we can simply log the attack action
        Debug.Log($"Melee attack executed towards {targetPosition} dealing {damage} damage.");
        // Here you would typically implement logic to check for enemies in range
        // and apply damage accordingly.
    }
}
