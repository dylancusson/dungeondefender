using UnityEngine;
using System.Collections; // Required for Invoke/Coroutines
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.UIElements; // Required for Lists

public class Character : MonoBehaviour
{
    // References to other components
    private CharacterStats stats;
    private CharacterHealth health;
    private CharacterTargeting targeting;
    private NavMeshAgent agent;

    private BasicAttack basicAttack;
    private SpecialAttack specialAttack;

    public Animator animator;

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
        animator.SetBool("isIdle", true);
        basicAttack = GetComponent<BasicAttack>();
        specialAttack = GetComponent<SpecialAttack>();

        if (agent == null) Debug.LogError("NavMeshAgent component missing from " + gameObject.name);
        if (basicAttack == null) Debug.LogError("BasicAttack component missing from " + gameObject.name);
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
                //Update animation to idle
                SetAnimation("Idle");
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
                //Update animation to moving
                SetAnimation("Moving");

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
                }
                break;

            case State.special:
                specialAttack.SpecialATK();
                Debug.Log(gameObject.name + ": Using special ability!");
                stats.currentMana = 0;
                SetState(State.moving); // Or idle, depending on your design
                break;

            case State.dead:
                // Die(); // Call death logic like adding score, playing animation, etc
                SetAnimation("Dead");
                Debug.Log(gameObject.name + " is dead and will be destroyed.");
                Invoke("OnDeath", 1f); // Delay to allow death animation
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

        Debug.Log(gameObject + " is Attacking target: " + targeting.target.name + " for " + stats.currentAttackDmg + " damage.");
        basicAttack.Attack(targeting.target.transform.position);
        stats.GainMana();

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

    void OnDeath()
    {
        Destroy(gameObject);
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

    public void SetAnimation(string animationName)
    {
        switch (animationName)
        {
            case "Idle":
                animator.SetBool("isIdle", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", false);
                break;
            case "Moving":
                animator.SetBool("isIdle", false);
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
                break;
            case "Attacking":
                animator.SetBool("isIdle", false);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);
                break;
            case "Dead":
                animator.SetBool("isIdle", false);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", true);
                break;

        }
    }
}