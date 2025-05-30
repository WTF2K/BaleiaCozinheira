using UnityEngine;

public class EnemyBehaviour1 : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 1.0f;     // Antes era 0.5f � aumenta a altura da oscila��o
    public float floatFrequency = 1.5f;     // Antes era 1f � aumenta a velocidade da oscila��o

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Randomiza leve varia��o por inimigo
        floatAmplitude += Random.Range(-0.2f, 0.3f);
        floatFrequency += Random.Range(-0.2f, 0.4f);

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

