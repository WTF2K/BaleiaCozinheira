using UnityEngine;

public class EnemyBehaviour1 : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 1.0f;     // Antes era 0.5f — aumenta a altura da oscilação
    public float floatFrequency = 1.5f;     // Antes era 1f — aumenta a velocidade da oscilação

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Randomiza leve variação por inimigo
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

