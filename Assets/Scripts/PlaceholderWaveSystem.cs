using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceholderWaveSystem : MonoBehaviour
{

    //I just made this so I can actually make sure the wave counter works, feel free to change - Ryan
    private int currentWave;

    [SerializeField] public int lastWave;

    [Header("Where to show current wave")]

    [SerializeField] TextMeshProUGUI waveCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWave = 1;
    }

    void NextWave()
    {
        currentWave++;
        if (currentWave > lastWave)
        {
            Debug.Log("LevelDone!");
            SceneManager.LoadSceneAsync("YouWin");
        }
    }

    // Update is called once per frame
    void Update()
    {
        waveCounter.text = "Wave: " + currentWave.ToString() + "/" + lastWave.ToString();
    }
}
