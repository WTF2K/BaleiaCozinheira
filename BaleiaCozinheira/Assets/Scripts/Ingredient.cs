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
            // Troca o segmento do mapa
            Segments segments = Object.FindAnyObjectByType<Segments>();
            if (segments != null)
            {
                // Avança para o segmento correspondente ao ingrediente coletado
                if (spawner != null)
                {
                    int segmentToActivate = spawner.GetCurrentIngredientIndex() + 1;
                    segments.SetSegmentIndex(segmentToActivate);
                }

                // Reposiciona o player para o início do novo segmento
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

            // Informa ao spawner que o ingrediente foi coletado
            if (spawner != null)
                spawner.IngredientCollected();
            
            // Destroi o ingrediente
            Destroy(gameObject);
        }
    }
}