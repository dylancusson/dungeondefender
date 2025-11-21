using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [Tooltip("Damage dealt when something steps on the spikes.")]
    public int damage = 15;

    [Tooltip("If true, the trap only triggers once and then stays inactive forever.")]
    public bool triggerOnce = false;

    [Tooltip("Time in seconds before the trap can trigger again (if not one-shot).")]
    public float rearmDelay = 2f;

    [Header("Targeting")]
    [Tooltip("Only objects with this tag are damaged. Leave empty to damage anything with CharacterHealth.")]
    public string targetTag = "Enemy";    // e.g. "Player" or "Enemy"

    [Header("Animation (optional)")]
    [Tooltip("Optional Animator for spike up/down animation.")]
    public Animator animator;

    [Tooltip("Trigger name to play when the trap activates (e.g. 'Activate').")]
    public string activateTriggerName = "Activate";

    private bool _canTrigger = true;
    private Collider2D _collider;

    private void Reset()
    {
        // Make sure collider is a trigger so objects can pass through but still be detected.
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        Debug.Log(Equals(_collider, null) ? "No Collider2D found!" : "Collider2D found.");
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        if (_collider != null)
            _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_canTrigger) return;
        Debug.Log(other.name + " stepped on SpikeTrap.");
        // Check for CharacterHealth on this object or its parent (same pattern as FireTrap).
        CharacterHealth ch = other.GetComponent<CharacterHealth>();
        if (ch == null) return; // nothing to damage

        // If a targetTag was specified, only damage objects with that tag.
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag))
            return;

        // Deal damage using your existing health system.
        ch.TakeDamage(damage);

        // Play spike animation if provided.
        if (animator != null && !string.IsNullOrEmpty(activateTriggerName))
        {
            animator.SetTrigger(activateTriggerName);
        }

        // Handle trap state after triggering.
        if (triggerOnce)
        {
            _canTrigger = false;
            if (_collider != null)
                _collider.enabled = false; // permanently disabled
        }
        else
        {
            _canTrigger = false;
            // Rearm after a delay so it doesn't spam damage every frame.
            Invoke(nameof(Rearm), rearmDelay);
        }
    }

    private void Rearm()
    {
        _canTrigger = true;
        if (_collider != null && !_collider.enabled)
            _collider.enabled = true;
    }
}
