using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currencies : MonoBehaviour
{
    private int playerGold;
    private int playerMana;

    [Header("Amount at start")]

    [SerializeField] int startingGold;
    [SerializeField] int startingMana;

    [Header("Where to show current amount")]

    [SerializeField] TextMeshProUGUI goldUI;
    [SerializeField] TextMeshProUGUI manaUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerGold = startingGold;
        playerMana = startingMana;
    }

    // Update is called once per frame
    void Update()
    {
        goldUI.text = playerGold.ToString();
        manaUI.text = playerMana.ToString();
    }

    public void gainMoney(int amountToGet)
    {
        playerGold += amountToGet;
    }

    public void gainMana(int amountToGet)
    {
        playerMana += amountToGet;
    }

    public bool SpendMoney(int amountToSpend)
    {
        if( amountToSpend > playerGold)
        {
            return false;
        }
        else
        {
            playerGold -= amountToSpend;
            return true;
        }
    }

    public bool SpendMana(int amountToSpend)
    {
        if (amountToSpend > playerMana)
        {
            return false;
        }
        else
        {
            playerMana -= amountToSpend;
            return true;
        }

    }
}
