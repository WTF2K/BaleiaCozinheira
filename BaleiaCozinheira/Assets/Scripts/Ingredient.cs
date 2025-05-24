using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private Material originalMaterial;
    private Renderer rend;
    private static int currentIngredientIdx = 0;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;
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
                // Avança para o próximo segmento
                currentIngredientIdx = Mathf.Min(currentIngredientIdx + 1, 6); // Vai até o Element 6
                segments.SetSegmentIndex(currentIngredientIdx);

                // Reposiciona o player para o início do novo segmento
                // Usa o próprio 'other.transform' pois é o player
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
        }
    }
}