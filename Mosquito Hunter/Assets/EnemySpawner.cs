using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public GameObject bossPrefab;

    public Transform spawnPoint;          // điểm spawn chính
    public float spawnInterval = 1f;    // thời gian giữa mỗi enemy
    public float minSpawnInterval = 0.3f; // thời gian nhỏ nhất giữa các enemy
    public float spawnIntervalDecrease = 0.1f; // giảm bao nhiêu mỗi wave

    public int enemiesPerWave = 5;        // số enemy ban đầu mỗi wave
    public float waveDelay = 5f;          // thời gian giữa các wave
    private int currentWave = 1;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        int enemiesToSpawn = enemiesPerWave + (currentWave - 1) * 3; // wave sau nhiều enemy hơn

        // Tính spawnInterval giảm dần theo wave
        float currentSpawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - (currentWave - 1) * spawnIntervalDecrease);

        Debug.Log($"Wave {currentWave} - Spawning {enemiesToSpawn} enemies with interval {currentSpawnInterval}");

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnInterval);
        }

        currentWave++;
        yield return new WaitForSeconds(waveDelay);

        // Tiếp tục spawn wave tiếp theo
        StartCoroutine(SpawnWave());
    }

    void SpawnEnemy()
    {
        int rand = Random.Range(1, 5); // 1-Enemy1, 2-Enemy2, 3-Enemy3, 4-Boss
        GameObject enemyToSpawn = null;

        switch (rand)
        {
            case 1: enemyToSpawn = enemy1Prefab; break;
            case 2: enemyToSpawn = enemy2Prefab; break;
            case 3: enemyToSpawn = enemy3Prefab; break;
            case 4: enemyToSpawn = bossPrefab; break;
        }

        if (enemyToSpawn != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : new Vector3(-13.3f, -1.5f, 0f);
            Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Enemy prefab chưa được gán!");
        }
    }
}
