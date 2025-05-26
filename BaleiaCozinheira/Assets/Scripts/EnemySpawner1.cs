using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner1 : MonoBehaviour
{
    public enum SpawnMode { Grid, Random }

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Player & Camera")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;

    [Header("Spawn Mode")]
    [SerializeField] private SpawnMode spawnMode = SpawnMode.Grid;

    [Header("Grid Settings (only for Grid Mode)")]
    [SerializeField] private int gridRows = 3;
    [SerializeField] private int gridCols = 3;
    [SerializeField] private float gridWidth = 12f;
    [SerializeField] private float gridHeight = 8f;

    [Header("Spawn General Settings")]
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private float difficultyRampTime = 60f; // Tempo até alcançar minSpawnInterval

    private float gameStartTime;
    private float nextSpawnTime;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        gameStartTime = Time.time;
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            if (spawnMode == SpawnMode.Grid)
                SpawnEnemiesGridMode();
            else
                SpawnEnemiesRandomMode();

            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        float t = (Time.time - gameStartTime) / difficultyRampTime;
        float interval = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, t);
        nextSpawnTime = Time.time + interval;
    }

    void SpawnEnemyAtPosition(Vector3 pos)
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        // Rotação 180 graus no eixo Y
        GameObject enemy = Instantiate(enemyPrefabs[index], pos, Quaternion.Euler(0, 180, 0));
        spawnedEnemies.Add(enemy);
    }

    void SpawnEnemiesGridMode()
    {
        Vector3 center = playerTransform.position + Vector3.forward * spawnDistance;

        List<Vector3> gridPositions = new List<Vector3>();
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                float offsetX = (col - 1) * (gridWidth / (gridCols - 1));
                float offsetY = (1 - row) * (gridHeight / (gridRows - 1));
                Vector3 spawnPos = center + new Vector3(offsetX, offsetY, 0);
                gridPositions.Add(spawnPos);
            }
        }

        List<Vector3> selectedPositions = new List<Vector3>();
        while (selectedPositions.Count < 4 && gridPositions.Count > 0)
        {
            int randIndex = Random.Range(0, gridPositions.Count);
            selectedPositions.Add(gridPositions[randIndex]);
            gridPositions.RemoveAt(randIndex);
        }

        foreach (Vector3 pos in selectedPositions)
        {
            SpawnEnemyAtPosition(pos);
        }
    }

    void SpawnEnemiesRandomMode()
    {
        Vector3 spawnCenter = playerTransform.position + Vector3.forward * spawnDistance;

        for (int i = 0; i < 4; i++)
        {
            float offsetX = Random.Range(-gridWidth / 2, gridWidth / 2);
            float offsetY = Random.Range(-gridHeight / 2, gridHeight / 2);
            Vector3 spawnPos = spawnCenter + new Vector3(offsetX, offsetY, 0);

            SpawnEnemyAtPosition(spawnPos);
        }
    }

    public void ResetSpawner()
    {
        // Destrói todos os inimigos ativos
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();

        // Reinicia o temporizador para o spawn
        gameStartTime = Time.time;
        ScheduleNextSpawn();
    }
}
