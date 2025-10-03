using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    // Reference to the stats component
    private CharacterStats stats;

    // Health variables are now managed here
    public int currentHealth;
    public bool isAlive = true;

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        if (stats != null)
        {
            currentHealth = stats.maxHealth;
        }
    }

    // Public function to receive damage from external sources (e.g., Attack calls)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Took damage: " + damage + ", Current Health: " + currentHealth);

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