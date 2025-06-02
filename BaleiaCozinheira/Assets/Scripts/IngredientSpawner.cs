using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Ingredient Prefabs")]
    [SerializeField] private GameObject[] ingredientPrefabs = new GameObject[7];

    [Header("Spawn Timers (seconds)")]
    [SerializeField] private float[] spawnTimers = new float[7] { 2f, 3f, 4f, 5f, 6f, 7f, 8f };

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 80f;
    [SerializeField] private float spawnHeight = 2f;
    [SerializeField] private float spawnWidth = 10f;
    [SerializeField] private float respawnTime = 5f;

    [Header("Player Reference")]
    [SerializeField] private Transform player; // <-- Adicione esta linha

    private static int currentIngredientIndex = 0;
    private float nextSpawnTime;
    private bool hasActiveIngredient = false;

    void Start()
    {
        // Remova a linha que busca pela tag
        // player = GameObject.FindWithTag("Player")?.transform;
        nextSpawnTime = Time.time + spawnTimers[0];
        IngredientManager.Instance?.AtualizarIngredienteAtual(currentIngredientIndex);
    }

    void Update()
    {
        if (!hasActiveIngredient && Time.time >= nextSpawnTime && currentIngredientIndex < ingredientPrefabs.Length)
        {
            SpawnCurrentIngredient();
        }
    }

    void SpawnCurrentIngredient()
    {
        if (ingredientPrefabs[currentIngredientIndex] != null && player != null)
        {
            Vector3 spawnPosition = player.position + player.forward * spawnDistance;
            spawnPosition.y = player.position.y + spawnHeight;
            spawnPosition.x += Random.Range(-spawnWidth, spawnWidth);

            GameObject ingredient = Instantiate(ingredientPrefabs[currentIngredientIndex], spawnPosition, Quaternion.identity);
            ingredient.GetComponent<Ingredient>().SetSpawner(this);
            hasActiveIngredient = true;
        }
    }

    public void IngredientCollected()
    {
        currentIngredientIndex++;
        hasActiveIngredient = false;

        if (currentIngredientIndex < ingredientPrefabs.Length)
        {
            nextSpawnTime = Time.time + spawnTimers[currentIngredientIndex];
        }
    }

    public void IngredientMissed()
    {
        hasActiveIngredient = false;
        nextSpawnTime = Time.time + respawnTime;
    }

    public int GetCurrentIngredientIndex()
    {
        return currentIngredientIndex;
    }
}