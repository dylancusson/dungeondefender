using UnityEngine;

/// <summary>
/// Abstract base class for all character attacks.
/// Defines common properties and the mandatory ExecuteAttack method.
/// </summary>
public abstract class BasicAttack : MonoBehaviour
{
    [Header("Core Attack Properties")]
    [Tooltip("The designated point from which the projectile or effect originates.")]
    // This variable is inherited by all derived classes (like ArrowBasicAttack)
    public Transform spawnPoint;

    [Tooltip("How much damage this attack deals.")]
    public float damage = 1f;

    /// <summary>
    /// This abstract method MUST be implemented by all attack subclasses
    /// to define the specific behavior of the attack (e.g., launching a projectile).
    /// </summary>
    /// <param name="targetPosition">The desired world position to attack.</param>
    public abstract void Attack(Vector3 targetPosition);
}
