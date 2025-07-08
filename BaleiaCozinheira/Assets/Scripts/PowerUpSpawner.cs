using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public GameObject[] powerUpPrefabs;
    public float spawnIntervalMin = 5f;
    public float spawnIntervalMax = 10f;

    [Header("Player")]
    public Transform baleiaTransform;
    public float spawnDistance = 10f;

    [Header("Despawn Settings")]
    public float despawnBehindDistance = 5f;

    private List<GameObject> spawnedPowerUps = new List<GameObject>();

    void Start()
    {
        Debug.Log("PowerUpSpawner iniciado");

        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
            Debug.LogWarning("Nenhum prefab de power-up atribuído ao PowerUpSpawner!");

        if (baleiaTransform == null)
            Debug.LogWarning("baleiaTransform não atribuído ao PowerUpSpawner!");

        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        DespawnPassedPowerUps();
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            Debug.Log($"Aguardando {waitTime} segundos para spawnar power-up...");
            yield return new WaitForSeconds(waitTime);

            SpawnPowerUp();
        }
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0 || baleiaTransform == null)
        {
            Debug.LogWarning("PowerUpSpawner não configurado corretamente!");
            return;
        }

        int index = Random.Range(0, powerUpPrefabs.Length);
        Vector3 forward = baleiaTransform.forward;
        Vector3 spawnPos = baleiaTransform.position + forward * spawnDistance;
        spawnPos.y = baleiaTransform.position.y;

        Quaternion rotation = Quaternion.Euler(0, 180, 0);

        GameObject powerUp = Instantiate(powerUpPrefabs[index], spawnPos, rotation);
        spawnedPowerUps.Add(powerUp);

        Debug.Log($"Spawnando power-up '{powerUp.name}' em {spawnPos} com rotação Y=180");
    }

    void DespawnPassedPowerUps()
    {
        for (int i = spawnedPowerUps.Count - 1; i >= 0; i--)
        {
            GameObject p = spawnedPowerUps[i];
            if (p != null && p.transform.position.z < baleiaTransform.position.z - despawnBehindDistance)
            {
                Destroy(p);
                spawnedPowerUps.RemoveAt(i);
                Debug.Log($"Power-up '{p.name}' destruído por estar atrás do jogador.");
            }
        }
    }

    public void ResetSpawner()
    {
        // Destroi todos os power-ups ainda ativos
        foreach (var p in spawnedPowerUps)
        {
            if (p != null) Destroy(p);
        }
        spawnedPowerUps.Clear();
        Debug.Log("Todos os power-ups foram destruídos.");
    }
}

