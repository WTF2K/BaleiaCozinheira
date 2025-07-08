using UnityEngine;

public class TintaColisao : MonoBehaviour
{
    public float duracaoEscurecer = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verifica que é mesmo a baleia
            BaleiaSeguirRato baleia = other.GetComponent<BaleiaSeguirRato>();
            if (baleia != null)
            {
                Debug.Log("Colisão com tinta confirmada.");
                HUDManager.Instance.EscurecerHUD(duracaoEscurecer);
                Destroy(gameObject);
            }
        }
    }
}