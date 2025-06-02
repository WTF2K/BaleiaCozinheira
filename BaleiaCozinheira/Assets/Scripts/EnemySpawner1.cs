using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner1 : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Player & Camera")]
    [SerializeField] private Transform playerTransform;

    [Header("Spawn General Settings")]
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private int minEnemiesPerWave = 1;
    [SerializeField] private int maxEnemiesPerWave = 4;
    [SerializeField] private float spawnDistance = 10f;

    [Header("Despawn Settings")]
    [SerializeField] private float despawnBehindDistance = 5f;

    private float nextSpawnTime;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWave();
            ScheduleNextSpawn();
        }
        DespawnPassedEnemies();
    }

    void ScheduleNextSpawn()
    {
        // delega cálculo de dificuldade ao DifficultyManager
        float t = DifficultyManager.Instance.NormalizedDifficulty;     // valor entre 0 e 1
        float interval = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, t);
        nextSpawnTime = Time.time + interval;
    }

    void SpawnWave()
    {
        // quantos inimigos nesta onda?
        float t = DifficultyManager.Instance.NormalizedDifficulty;
        int count = Mathf.RoundToInt(Mathf.Lerp(minEnemiesPerWave, maxEnemiesPerWave, t));

        Vector3 center = playerTransform.position + Vector3.forward * spawnDistance;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = center + new Vector3(
                Random.Range(-5f, 5f),       // ajuste horizontal se quiser
                Random.Range(-3f, 3f),
                0);
            SpawnEnemyAtPosition(pos);
        }
    }

    void SpawnEnemyAtPosition(Vector3 pos)
    {
        int idx = Random.Range(0, enemyPrefabs.Length);
        var e = Instantiate(enemyPrefabs[idx], pos, Quaternion.Euler(0,180,0));
        spawnedEnemies.Add(e);
    }

    void DespawnPassedEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            var e = spawnedEnemies[i];
            if (e != null && e.transform.position.z < playerTransform.position.z - despawnBehindDistance)
            {
                Destroy(e);
                spawnedEnemies.RemoveAt(i);
            }
        }
    }

    public void ResetSpawner()
    {
        spawnedEnemies.ForEach(e => { if (e) Destroy(e); });
        spawnedEnemies.Clear();
        ScheduleNextSpawn();
    }
    public List<GameObject> GetSpawnedEnemies()
    {
        return spawnedEnemies;
    }

}
