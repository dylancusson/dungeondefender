using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    // Reference to the stats component
    private CharacterStats stats;
    private Animator animator;

    // Health variables are now managed here
    public int currentHealth;
    public int maximumHealth;
    public bool isAlive = true;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();

        if (stats != null)
        {
            maximumHealth = stats.maxHealth;
            currentHealth = maximumHealth;
            healthBar.updateHealthBar(currentHealth, maximumHealth);
        }
    }

    // Public function to receive damage from external sources (e.g., Attack calls)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("isHit");
        Debug.Log("Took damage: " + damage + ", Current Health: " + currentHealth);
        healthBar.updateHealthBar(currentHealth, maximumHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Helper method for the AI to check status
    public bool IsDead()
    {
        return !isAlive;
    }

    private void Die()
    {
        currentHealth = 0;
        isAlive = false;

        // Notify the main controller (or state machine) of death
        Character characterController = GetComponent<Character>();
        if (characterController != null)
        {
            characterController.SetState(Character.State.dead);
        }

        Debug.Log(gameObject.name + " has died!");
        // We will let the Character controller handle the Destroy logic
    }
}