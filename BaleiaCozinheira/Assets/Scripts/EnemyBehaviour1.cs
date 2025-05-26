using UnityEngine;

public class EnemyBehaviour1 : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 0.5f;     // Altura da oscilação
    public float floatFrequency = 1f;       // Velocidade da oscilação

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Garante que o inimigo está virado para trás (180° no eixo Y)
        transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    void Update()
    {
        FloatMotion();
    }

    void FloatMotion()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BaleiaSeguirRato baleia = other.GetComponent<BaleiaSeguirRato>();
            if (baleia != null)
            {
                baleia.ReduceSpeed(0.3f);
            }
        }
    }

}

