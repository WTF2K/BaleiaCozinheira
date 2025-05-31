using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 5f;
    public float spawnDistanceAhead = 30f;
    public float spawnAreaWidth = 10f;
    public float spawnHeight = 2f;
    public int maxCoinsOnScene = 10;

    private Transform player;
    private float nextSpawnTime = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null || coinPrefab == null) return;

        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("Coin").Length < maxCoinsOnScene)
        {
            SpawnCoin();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnCoin()
    {
        Vector3 spawnPos = player.position + player.forward * spawnDistanceAhead;
        spawnPos.x += Random.Range(-spawnAreaWidth, spawnAreaWidth);
        spawnPos.y += spawnHeight;

        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
