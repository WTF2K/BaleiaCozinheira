using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;
    public Camera mainCamera;

    public float spawnInterval = 1f;
    public float spawnDistanceAhead = 20f;

    void Start()
    {
        if (player == null || mainCamera == null)
        {
            Debug.LogError("🚫 CoinSpawner: player ou camera não atribuídos!");
            return;
        }

        InvokeRepeating(nameof(SpawnCoin), 1f, spawnInterval);
    }

    void SpawnCoin()
    {
        // Viewport coords (0..1) - zona segura no ecrã
        float viewportX = Random.Range(0.4f, 0.6f);
        float viewportY = Random.Range(0.4f, 0.6f);

        float distanceAhead = spawnDistanceAhead;

        Vector3 viewportPoint = new Vector3(viewportX, viewportY, distanceAhead); // distância relativa à câmara

        Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewportPoint);

        Instantiate(coinPrefab, worldPos, Quaternion.identity);

        Debug.Log($"🪙 Moeda criada corretamente no mundo: {worldPos}");
    }

}
