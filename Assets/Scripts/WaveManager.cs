using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Next Level/WinScreen")]
    [SerializeField] private string nextScene;

    [Header("Win/Lose SFX")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float scalingFactor = 0.75f;
    [SerializeField] public static int maxWaves = 3;
    [SerializeField] private float spawnRadius = 4f;

    [Header("Indicators")]
    [SerializeField] GameObject enemySpawn;
    [SerializeField] GameObject enemyGoal;

    private static int currentWave = 0;
    private int enemiesToSpawn;
    public static int enemyCount = 0;

    // Singleton instance
    public static WaveManager instance;
    private void Awake()
    {
        // Implement singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        waveText.text = "Wave: 0/" + maxWaves;
        enemySpawn.SetActive(true);
        enemyGoal.SetActive(true);
    }

    public void StartWave()
    {
        if (currentWave >= maxWaves)
        {
            Debug.Log("All waves completed!");
            return;
        }

        if(currentWave == 0)
        {
            enemySpawn.SetActive(false);
            enemyGoal.SetActive(false);
        }

        currentWave++;
        waveText.text = "Wave: " + currentWave + "/" + maxWaves;
        enemiesToSpawn = Mathf.CeilToInt(enemiesPerWave * (1 + (currentWave - 1) * scalingFactor));

        while (enemiesToSpawn > 0 && currentWave <= maxWaves)
        {
            SpawnEnemy();
            enemiesToSpawn--;
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        Vector3 spawnPosition = GetSpawnPosition();
        Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        // 1. Pick a random point in the radius (World Space)
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 randomWorldPoint = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnRadius;

        // 2. Snap that point to the nearest Tile Grid coordinate
        Vector3Int cellPosition = targetTilemap.WorldToCell(randomWorldPoint);

        // 3. Convert that Grid coordinate back to the center of the cell in World Space
        return targetTilemap.GetCellCenterWorld(cellPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public static void GameOver()
    {
        SceneController.instance.NextScene("YouLose");
    }

    public void Update()
    {
        if (enemyCount <= 0 && currentWave >= maxWaves)
        {
            Debug.Log("All waves completed! You win!");
            SceneController.instance.NextScene(nextScene);
            currentWave = 0;
        }

        else if (enemyCount <= 0 && currentWave >= 1)
        {
            Debug.Log("Starting the next wave!");
            //Invoke("StartWave", 3.0f);
            StartWave();
        }
    }

    /*public void NextScene()
    {
        SceneManager.LoadSceneAsync(nextScene);
        currentWave = 0;
    }*/
}
