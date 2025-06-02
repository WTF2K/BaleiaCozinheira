using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private Material originalMaterial;
    private Renderer rend;
    private IngredientSpawner spawner;
    private float lifeTime = 30f;
    private float spawnTime;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - spawnTime >= lifeTime)
        {
            if (spawner != null)
                spawner.IngredientMissed();
            Destroy(gameObject);
        }
    }

    public void SetSpawner(IngredientSpawner ingredientSpawner)
    {
        spawner = ingredientSpawner;
    }

    public void ResetMaterial()
    {
        if (rend != null && originalMaterial != null)
            rend.material = originalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int currentIndex = spawner.GetCurrentIngredientIndex();

            IngredientManager.Instance?.RegistarIngredienteApanhado(currentIndex);

            Segments segments = Object.FindAnyObjectByType<Segments>();
            if (segments != null)
            {
                if (spawner != null)
                {
                    int segmentToActivate = currentIndex + 1;
                    segments.SetSegmentIndex(segmentToActivate);
                }

                Transform playerTransform = other.transform;
                if (playerTransform != null)
                {
                    GameObject[] ativos = segments.GetActiveSegmentsArray();
                    if (ativos.Length > 0 && ativos[0] != null)
                    {
                        Vector3 pos = ativos[0].transform.position;
                        pos.y = playerTransform.position.y;
                        playerTransform.position = pos;
                    }
                    else
                    {
                        playerTransform.position = Vector3.zero;
                    }
                }
            }

            if (spawner != null)
            {
                spawner.IngredientCollected();
                IngredientManager.Instance?.AtualizarIngredienteAtual(spawner.GetCurrentIngredientIndex());
            }

            Destroy(gameObject);
        }
    }
}