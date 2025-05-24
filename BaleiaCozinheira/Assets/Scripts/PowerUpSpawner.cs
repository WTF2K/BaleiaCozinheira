using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Arrasta aqui os prefabs dos power-ups
    public float spawnIntervalMin = 5f;
    public float spawnIntervalMax = 10f;

    public Transform baleiaTransform; // Arraste aqui o objeto da baleia no Inspector
    public float spawnDistance = 10f; // Distância à frente da baleia para spawnar

    void Start()
    {
        Debug.Log("PowerUpSpawner iniciado");
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            Debug.LogWarning("Nenhum prefab de power-up atribuído ao PowerUpSpawner!");
        }
        if (baleiaTransform == null)
        {
            Debug.LogWarning("baleiaTransform não atribuído ao PowerUpSpawner!");
        }
        StartCoroutine(SpawnRoutine());
    }

    System.Collections.IEnumerator SpawnRoutine()
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
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            Debug.LogWarning("powerUpPrefabs está vazio ou nulo!");
            return;
        }
        if (baleiaTransform == null)
        {
            Debug.LogWarning("baleiaTransform não atribuído, não é possível spawnar power-up!");
            return;
        }

        int index = Random.Range(0, powerUpPrefabs.Length);

        // Calcula a posição à frente da baleia
        Vector3 forward = baleiaTransform.forward;
        Vector3 spawnPos = baleiaTransform.position + forward * spawnDistance;

        // Mantém o mesmo Y da baleia (opcional, ajuste se necessário)
        spawnPos.y = baleiaTransform.position.y;

        // Define rotação Y = 180
        Quaternion rotation = Quaternion.Euler(0, 180, 0);

        Debug.Log($"Spawnando power-up '{powerUpPrefabs[index].name}' em {spawnPos} com rotação Y=180");
        Instantiate(powerUpPrefabs[index], spawnPos, rotation);
    }
}