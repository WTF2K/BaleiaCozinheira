using UnityEngine;

public class TintaAutoDestruir : MonoBehaviour
{
    public float tempoDestruir = 3f;

    void Start()
    {
        Destroy(gameObject, tempoDestruir);
    }
}
