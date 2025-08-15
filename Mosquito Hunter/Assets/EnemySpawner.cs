using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public GameObject bossPrefab;

    public GameObject spawnPoint; // GameObject làm vị trí spawn chung

    public float spawnInterval = 2f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        int rand = Random.Range(1, 5); // 1 - Enemy1, 2 - Enemy2, 3 - Enemy3, 4 - Boss
        GameObject enemyToSpawn = null;

        switch (rand)
        {
            case 1:
                enemyToSpawn = enemy1Prefab;
                break;
            case 2:
                enemyToSpawn = enemy2Prefab;
                break;
            case 3:
                enemyToSpawn = enemy3Prefab;
                break;
            case 4:
                enemyToSpawn = bossPrefab;
                break;
        }

        if (enemyToSpawn != null && spawnPoint != null)
        {
            // Spawn tất cả enemy tại vị trí của spawnPoint
            Instantiate(enemyToSpawn, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
