using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam == null) return;

        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);

        // Verifica se está atrás da câmara (Z < 0) ou fora da viewport
        if (viewportPos.z < 0 ||
            viewportPos.x < 0 || viewportPos.x > 1 ||
            viewportPos.y < 0 || viewportPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // Reproduzir som da moeda antes de destruir o objeto
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

            CoinManager.Instance?.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
