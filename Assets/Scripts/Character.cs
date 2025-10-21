using UnityEngine;
using System.Collections; // Required for Invoke/Coroutines
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Tilemaps;
using UnityEngine.AI; // Required for Lists

public class Character : MonoBehaviour
{
    // References to other components
    private CharacterStats stats;
    private CharacterHealth health;
    private CharacterTargeting targeting;
    private NavMeshAgent agent;

    // Path recalculation control
    public float pathRecalculationRate = 1f; // seconds

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
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("NavMeshAgent component missing from " + gameObject.name);
        agent.autoBraking = false; // For continuous movement
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
                // FindTarget logic may change the state to moving
                targeting.FindTarget();

                // If a target was found, initiate pathfinding
                if (currentState == State.moving) // targeting.FindTarget() likely calls SetState(State.moving)
                {
                    // We need the target's position to start pathfinding
                    StartMoveToTarget(targeting.target.transform.position);
                }
                break;

            case State.moving:
                if (targeting.target == null)
                {
                    agent.ResetPath(); // Stop the agent
                    SetState(State.idle);
                }
                else if (targeting.IsTargetInAttackRange())
                {
                    agent.ResetPath(); // Stop the agent
                    SetState(State.attacking);
                }
                else
                {
                    // Simple Repath Check (No need for complicated timers!)
                    if (agent.remainingDistance < 1f || Time.time >= pathRecalculationRate)
                    {
                        // Recalculate if far away, OR if the path is almost done
                        StartMoveToTarget(targeting.target.transform.position);
                        pathRecalculationRate = Time.time + pathRecalculationRate;
                    }
                }
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
                // Die(); // Call death logic like adding score, playing animation, etc
                Destroy(gameObject);
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

    public void StartMoveToTarget(Vector3 targetPosition)
    {
        if (agent == null || !agent.isOnNavMesh) {
            Debug.LogWarning("Agent not on NavMesh or is null");
            return;
        }
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(targetPosition);
            SetState(State.moving);
        }
    }

    
}