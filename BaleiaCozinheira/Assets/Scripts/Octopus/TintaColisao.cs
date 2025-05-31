using UnityEngine;

public class TintaColisao : MonoBehaviour
{
    public float duracaoEscurecer = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(">> Tinta atingiu o player!");

            // Escurece o HUD através do HUDManager
            if (HUDManager.Instance != null)
                HUDManager.Instance.EscurecerHUD(duracaoEscurecer);
        }
    }
}