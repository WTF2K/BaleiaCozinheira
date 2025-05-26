using UnityEngine;

public class TintaColisao : MonoBehaviour
{
    public float duracaoEscurecer = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUDManager.Instance?.EscurecerHUD(duracaoEscurecer);
        }
    }
}
