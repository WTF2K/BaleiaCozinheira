using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject[] obstaclePrefabs;

    [Header("Player & Enemy Spawner Reference")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private EnemySpawner1 enemySpawner;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnInterval = 2f;
    [SerializeField] private float maxSpawnInterval = 6f;
    [SerializeField] private int minObstaclesPerWave = 1;
    [SerializeField] private int maxObstaclesPerWave = 3;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float minDistanceFromEnemies = 1.5f;

    [Header("Despawn Settings")]
    [SerializeField] private float despawnBehindDistance = 5f;

    private float nextSpawnTime;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            if (!IsEnemyWaveActive()) // só spawna obstáculos depois dos inimigos
            {
                SpawnObstacleWave();
                ScheduleNextSpawn();
            }
        }

        DespawnPassedObstacles();
    }

    void ScheduleNextSpawn()
    {
        float t = DifficultyManager.Instance.NormalizedDifficulty;
        float interval = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, t);
        nextSpawnTime = Time.time + interval;
    }

    void SpawnObstacleWave()
    {
        float t = DifficultyManager.Instance.NormalizedDifficulty;
        int count = Mathf.RoundToInt(Mathf.Lerp(minObstaclesPerWave, maxObstaclesPerWave, t));

        Vector3 center = playerTransform.position + Vector3.forward * spawnDistance;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos;
            int attempts = 0;
            do
            {
                pos = center + new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
                attempts++;
            } while (IsTooCloseToEnemies(pos) && attempts < 10);

            if (attempts < 10)
                SpawnObstacleAtPosition(pos);
        }
    }

    void SpawnObstacleAtPosition(Vector3 pos)
    {
        int idx = Random.Range(0, obstaclePrefabs.Length);
        var obstacle = Instantiate(obstaclePrefabs[idx], pos, Quaternion.identity);
        spawnedObstacles.Add(obstacle);
    }

    bool IsTooCloseToEnemies(Vector3 position)
    {
        foreach (var enemy in enemySpawner.GetSpawnedEnemies())
        {
            if (enemy != null && Vector3.Distance(position, enemy.transform.position) < minDistanceFromEnemies)
                return true;
        }
        return false;
    }

    bool IsEnemyWaveActive()
    {
        return enemySpawner.GetSpawnedEnemies().Exists(e => e != null);
    }

    void DespawnPassedObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            var o = spawnedObstacles[i];
            if (o != null && o.transform.position.z < playerTransform.position.z - despawnBehindDistance)
            {
                Destroy(o);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }

    public void ResetSpawner()
    {
        spawnedObstacles.ForEach(o => { if (o) Destroy(o); });
        spawnedObstacles.Clear();
        ScheduleNextSpawn();
    }
}
