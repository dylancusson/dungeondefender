using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackSpeed = 1f;
    public int health = 100;
    public float attackRange = 2f;
    public float detectionRadius = 10f;
    public string targetTag = "Player"; // Tag of the target to detect
    public bool isAlive = true;
    public bool canAttack = true;

    public Transform target; // Target location to move towards
    public enum State { idle, moving, attacking, special, dead }
    public State currentState = State.idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.idle:
                FindTarget();
                break;
            case State.moving:
                //MoveTowardsTarget();
                break;
            case State.attacking:
                //Attack();
                break;
            case State.special:
                //SpecialAbility();
                break;
            case State.dead:
                //Die();
                break;
        }
    }

    void FindTarget()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                target = hitCollider.transform;
                Debug.Log("Target found: " + target.name);
                currentState = State.moving;
                break;
            }
            
        }
    }
}
