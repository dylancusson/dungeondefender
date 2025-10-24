using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FireTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [Tooltip("Damage applied every tick to anything inside the area.")]
    public int damagePerTick = 5;

    [Tooltip("Seconds between damage ticks.")]
    public float tickInterval = 0.5f;

    [Tooltip("If > 0, the trap arms for this long before dealing damage.")]
    public float armDelay = 0.0f;

    [Tooltip("If > 0, the trap disables itself after this long (lifetime). 0 = infinite.")]
    public float activeDuration = 0f;

    [Tooltip("Optional cooldown after disabling. 0 = no cooldown")]
    public float cooldown = 0f;

    [Header("Targeting")]
    [Tooltip("Only objects with this tag are damaged. Leave empty to damage anything with CharacterHealth.")]
    public string targetTag = "";    // e.g. "Player" or "Enemy"

    [Header("FX (optional)")]
    public GameObject onVFX;         // flame particles / sprite
    public GameObject offVFX;        // unlit version
    public AudioSource sizzleSFX;

    private readonly HashSet<CharacterHealth> _inside = new HashSet<CharacterHealth>();
    private float _nextTickTime;
    private float _disableAtTime = -1f;
    private bool _armed = false;
    private bool _active = true;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        _inside.Clear();
        _armed = armDelay <= 0f;
        _active = true;
        _nextTickTime = Time.time + tickInterval;

        if (activeDuration > 0f)
            _disableAtTime = Time.time + armDelay + activeDuration;

        SetFX(_armed && _active);
    }

    private void Update()
    {
        // Handle arming delay
        if (!_armed && Time.time >= (Time.time + armDelay)) _armed = true; // (Handled by OnEnable above, kept for safety)

        // Handle lifetime → cooldown
        if (_active && _disableAtTime > 0f && Time.time >= _disableAtTime)
        {
            _active = false;
            SetFX(false);
            if (cooldown > 0f) Invoke(nameof(ReenableAfterCooldown), cooldown);
            return;
        }

        if (!_active || !_armed) return;

        // Tick damage
        if (Time.time >= _nextTickTime)
        {
            _nextTickTime = Time.time + tickInterval;
            TickDamage();
        }
    }

    private void ReenableAfterCooldown()
    {
        _active = true;
        _armed = armDelay <= 0f;
        _nextTickTime = Time.time + tickInterval;

        if (activeDuration > 0f)
            _disableAtTime = Time.time + armDelay + activeDuration;
        else
            _disableAtTime = -1f;

        SetFX(_armed && _active);
    }

    private void TickDamage()
    {
        if (_inside.Count == 0) return;

        foreach (var ch in _inside)
        {
            if (ch == null || ch.IsDead()) continue;
            ch.TakeDamage(damagePerTick); // uses your existing health pipeline
        }

        if (sizzleSFX && !_active) sizzleSFX.Stop();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var ch = other.GetComponentInParent<CharacterHealth>();
        if (ch == null) return;

        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;

        _inside.Add(ch);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var ch = other.GetComponentInParent<CharacterHealth>();
        if (ch == null) return;
        _inside.Remove(ch);
    }

    private void SetFX(bool enabled)
    {
        if (onVFX)  onVFX.SetActive(enabled);
        if (offVFX) offVFX.SetActive(!enabled);
        if (sizzleSFX)
        {
            if (enabled && !sizzleSFX.isPlaying) sizzleSFX.Play();
            else if (!enabled && sizzleSFX.isPlaying) sizzleSFX.Stop();
        }
    }
}
