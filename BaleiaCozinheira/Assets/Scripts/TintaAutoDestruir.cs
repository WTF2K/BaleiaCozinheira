using UnityEngine;

public class TintaAutoDestruir : MonoBehaviour
{
    public float tempoDesaparecer = 3f;

    void Start()
    {
        Destroy(gameObject, tempoDesaparecer);
    }
}
