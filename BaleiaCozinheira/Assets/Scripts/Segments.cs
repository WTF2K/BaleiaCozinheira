using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segements : MonoBehaviour
{
    public GameObject[] segment;
    [SerializeField] float segmentLength = 10f;
    [SerializeField] int maxSegments = 3;
    [SerializeField] Transform player; // arrasta a baleia aqui no Inspector
    [SerializeField] float spawnThreshold = 15f; // distância para spawnar novo segmento

    private Queue<GameObject> activeSegments = new Queue<GameObject>();
    private float nextZPos = 0f;

    void Start()
    {
        // Instancia segmentos iniciais
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // Verifica se o jogador está perto do fim do último segmento
        if (player != null && activeSegments.Count > 0)
        {
            GameObject lastSegment = null;
            foreach (var seg in activeSegments) lastSegment = seg; // obtém o último da fila
            float lastZ = lastSegment.transform.position.z;

            if (player.position.z > lastZ - spawnThreshold)
            {
                SpawnSegment();
            }
        }
    }

    void SpawnSegment()
    {
        int segmentNum = Random.Range(0, segment.Length);
        GameObject newSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, nextZPos), Quaternion.Euler(0, -90, 0));
        activeSegments.Enqueue(newSegment);
        nextZPos += segmentLength;

        // Remove o segmento mais antigo se exceder o máximo
        if (activeSegments.Count > maxSegments)
        {
            GameObject oldSegment = activeSegments.Dequeue();
            Destroy(oldSegment);
        }
    }
}
