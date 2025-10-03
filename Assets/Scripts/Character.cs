using UnityEngine;
using System.Collections; // Required for Invoke/Coroutines

public class Character : MonoBehaviour
{
    // References to other components
    private CharacterStats stats;
    private CharacterHealth health;
    private CharacterTargeting targeting;

    // Attack state control
    public bool canAttack = true;

    // Define the States
    public enum State { idle, moving, attacking, special, dead }
    public State currentState = State.idle;

    void Awake()
    {
        // Get references to all sibling components
        stats = GetComponent<CharacterStats>();
        health = GetComponent<CharacterHealth>();
        targeting = GetComponent<CharacterTargeting>();
    }

    // Public method to allow other components (like Health) to change the state
    public void SetState(State newState)
    {
        currentState = newState;
        Debug.Log(gameObject.name + " State changed to: " + currentState);
    }

    void Update()
    {
        
        switch (currentState)
        {
            case State.idle:
                targeting.FindTarget();
                break;

            case State.moving:
                if (targeting.target == null)
                {
                    SetState(State.idle);
                }
                else if (targeting.IsTargetInAttackRange())
                {
                    SetState(State.attacking);
                }
                // else { /* MoveTowardsTarget() using stats.currentMoveSpeed */ }
                break;

            case State.attacking:
                if (targeting.target == null || !targeting.IsTargetInAttackRange())
                {
                    SetState(State.moving); // Target moved out of range
                }
                else if (stats.currentMana >= stats.maxMana)
                {
                    SetState(State.special);
                }
                else if (canAttack)
                {
                    Attack();
                    stats.GainMana();
                }
                break;

            case State.special:
                // SpecialAbility();
                Debug.Log("Using special ability!");
                stats.currentMana = 0;
                SetState(State.moving); // Or idle, depending on your design
                break;

            case State.dead:
                // Die();
                Destroy(this);
                break;
        }
    }

    // Attacking logic remains here as it ties movement, stats, and health together
    void Attack()
    {
        if (targeting.target == null)
        {
            SetState(State.idle);
            return;
        }

        Debug.Log("Attacking target: " + targeting.target.name + " for " + stats.currentAttackDmg + " damage.");

        // **Damage Delegation:** Find the target's Health component and call TakeDamage
        CharacterHealth targetHealth = targeting.target.GetComponent<CharacterHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(stats.currentAttackDmg);
        }

        canAttack = false;
        // Schedule the next attack based on the current attack speed
        Invoke("ResetAttack", 1f / stats.currentAttackSpeed);

        // Go back to moving/idle to re-check target status/range immediately
        SetState(State.moving);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}