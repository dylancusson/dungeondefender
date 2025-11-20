using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float scalingFactor = 0.75f;
    [SerializeField] private int maxWaves = 3;
    [SerializeField] private float spawnRadius = 4f;

    private int currentWave = 0;
    private int enemiesToSpawn;

    private void Start()
    {
        waveText.text = "Wave: 0/" + maxWaves;
    }

    public void StartWave()
    {
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

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnRadius;
        Vector3Int cellPosition = targetTilemap.WorldToCell(spawnPosition);

        return cellPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
