using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 10f;
    private float timer;

    void Start()
    {
        timer = spawnInterval;
        SpawnIngredient();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnIngredient();
            timer = spawnInterval;
        }
    }

    void SpawnIngredient()
    {
        if (ingredientPrefab == null || spawnPoints.Length == 0) return;
        int idx = Random.Range(0, spawnPoints.Length);
        Instantiate(ingredientPrefab, spawnPoints[idx].position, Quaternion.identity);
    }
}
