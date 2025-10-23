using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 5f;
    public float baseAttackSpeed = 1f;
    public int maxHealth = 100;
    public int maxMana = 50;
    public int manaGainRate = 5;
    public int manaHitGainRate = 2;
    public int maxAttackDmg = 10;
    public float baseAttackRange = 2f;

    [Header("Current/Modifiable Stats")]
    // Other scripts will read and potentially modify these current values
    public float currentMoveSpeed;
    public float currentAttackSpeed;
    public int currentAttackDmg;
    public float currentAttackRange;
    public int currentMana;

    void Start()
    {
        // Initialize current stats from base stats
        currentAttackDmg = maxAttackDmg;
        currentAttackSpeed = baseAttackSpeed;
        currentMoveSpeed = baseMoveSpeed;
        currentAttackRange = baseAttackRange;
        currentMana = 0;
    }

    // Example function to gain mana
    public void GainMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        Debug.Log(gameObject + ": Current Mana: " + currentMana);
    }
    public void GainMana()
    {
        currentMana += manaGainRate;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        Debug.Log(gameObject + ": Current Mana: " + currentMana);
    }

    // You can add methods for buff/debuff management here.
}