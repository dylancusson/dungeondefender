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
    private CharacterStats stats;
    private CharacterHealth health;
    private CharacterTargeting targeting;
    private NavMeshAgent agent;
    private BasicAttack basicAttack;
    private SpecialAttack specialAttack;

    public Animator animator;

    // Control how often we scan for new enemies (Optimization)
    public float targetScanRate = 0.5f;
    private float nextTargetScanTime = 0f;

    public float pathRecalculationRate = 1f;
    private float nextPathRecalcTime = 0f;

    public bool canAttack = true;
    private bool hasPaidReward = false;

    public enum State { idle, moving, attacking, special, dead }
    public State currentState = State.idle;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        health = GetComponent<CharacterHealth>();
        targeting = GetComponent<CharacterTargeting>();
        agent = GetComponent<NavMeshAgent>();
        animator.SetBool("isIdle", true);
        basicAttack = GetComponent<BasicAttack>();
        specialAttack = GetComponent<SpecialAttack>();
        agent.autoBraking = false;
    }

    private void OnEnable()
    {   
        if (this.tag == "Enemy")
        {
            WaveManager.enemyCount++;
        }
            
    }

    private void OnDisable()
    {
        if (this.tag == "Enemy")
        {
            WaveManager.enemyCount--;
            //WaveManager.CheckWinCondition();
        }
        if (this.tag == "Nexus")
        {
            WaveManager.GameOver();
        }
    }
    // Public method to allow other components (like Health) to change the state
    public void SetState(State newState)
    {
        currentState = newState;
        Debug.Log(gameObject.name + " State changed to: " + currentState);
    }

    void Update()
    {
        // 1. Always check if target died or was destroyed externally
        if (targeting.target == null && currentState != State.idle && currentState != State.dead)
        {
            // If target disappears, immediately scan for a new one
            targeting.FindBestTarget();
        }

        switch (currentState)
        {
            case State.idle:
                SetAnimation("Idle");

                // Scan for targets periodically
                if (Time.time >= nextTargetScanTime)
                {
                    targeting.FindBestTarget();
                    nextTargetScanTime = Time.time + targetScanRate;
                }

                if (currentState == State.moving && targeting.target != null)
                {
                    StartMoveToTarget(targeting.target.transform.position);
                }
                break;

            case State.moving:
                SetAnimation("Moving");

                // 2. Scan for better targets while moving
                // This allows switching from Nexus -> Enemy if one appears
                if (Time.time >= nextTargetScanTime)
                {
                    targeting.FindBestTarget(); // This might change targeting.target!
                    nextTargetScanTime = Time.time + targetScanRate;
                }

                if (targeting.target == null)
                {
                    agent.ResetPath();
                    SetState(State.idle);
                }
                else if (targeting.IsTargetInAttackRange())
                {
                    agent.ResetPath();
                    SetState(State.attacking);
                }
                else
                {
                    // Movement Logic
                    if (agent.remainingDistance < 1f || Time.time >= nextPathRecalcTime)
                    {
                        // Update destination in case the target (Enemy) moved
                        StartMoveToTarget(targeting.target.transform.position);
                        nextPathRecalcTime = Time.time + pathRecalculationRate;
                    }
                }
                break;

            case State.attacking:
                // ... [Keep existing Attack logic] ...
                if (targeting.target == null || !targeting.IsTargetInAttackRange())
                {
                    SetState(State.moving);
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
                // ... [Keep existing Special logic] ...
                specialAttack.SpecialATK();
                stats.currentMana = 0;
                SetState(State.moving);
                break;

            case State.dead:
                // ... [Keep existing Dead logic] ...
                SetAnimation("Dead");
                if (!IsInvoking("OnDeath")) Invoke("OnDeath", 0.6f);
                if (!hasPaidReward)
                {
                    stats.GainGold();
                    hasPaidReward = true;
                }
                break;
        }
    }

    // Attacking logic remains here as it ties movement, stats, and health together
    void Attack()
    {
        if (targeting.target == null) { SetState(State.idle); return; }

        basicAttack.Attack(targeting.target.transform.position);
        stats.GainMana();

        CharacterHealth targetHealth = targeting.target.GetComponent<CharacterHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(stats.currentAttackDmg);
        }

        canAttack = false;
        Invoke("ResetAttack", 1f / stats.currentAttackSpeed);

        // Return to moving to re-evaluate position relative to target
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