using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segments : MonoBehaviour
{
    public GameObject[] segment;
    [SerializeField] float segmentLength = 10f;
    [SerializeField] int maxSegments = 3;
    [SerializeField] Transform player;
    [SerializeField] float spawnThreshold = 15f;

    private Queue<GameObject> activeSegments = new Queue<GameObject>();
    private GameObject lastSegment = null;
    private Queue<(GameObject, int)> segmentPool = new Queue<(GameObject, int)>();
    // Guarda todos os prefabs originais
    private GameObject[] allSegments;
    private int currentSegmentIdx = 0;

    void Start()
    {
        // Guarda todos os prefabs originais
        allSegments = (GameObject[])segment.Clone();
        // Começa só com o Element 0
        segment = new GameObject[] { allSegments[0] };
        currentSegmentIdx = 0;
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        if (player != null && lastSegment != null)
        {
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
        GameObject newSegment = null;

        // Pooling: reutiliza se possível, mas só se for do mesmo tipo
        if (segmentPool.Count > 0)
        {
            // Procura um segmento do mesmo tipo no pool
            int poolCount = segmentPool.Count;
            bool found = false;
            for (int i = 0; i < poolCount; i++)
            {
                var pooled = segmentPool.Dequeue();
                if (!found && pooled.Item2 == segmentNum)
                {
                    newSegment = pooled.Item1;
                    found = true;
                }
                else
                {
                    segmentPool.Enqueue(pooled);
                }
            }
        }

        Vector3 spawnPos;
        if (lastSegment == null)
        {
            spawnPos = Vector3.zero;
        }
        else
        {
            // Calcula a posição logo após o último segmento
            spawnPos = lastSegment.transform.position + new Vector3(0, 0, segmentLength);
        }

        if (newSegment == null)
        {
            newSegment = Instantiate(segment[segmentNum], spawnPos, Quaternion.Euler(0, -90, 0));
        }
        else
        {
            newSegment.transform.position = spawnPos;
            newSegment.transform.rotation = Quaternion.Euler(0, -90, 0);
            newSegment.SetActive(true);
        }

        activeSegments.Enqueue(newSegment);
        lastSegment = newSegment;

        if (activeSegments.Count > maxSegments)
        {
            GameObject oldSegment = activeSegments.Dequeue();
            oldSegment.SetActive(false);

            // Descobre o índice do prefab para o pooling correto
            int prefabIdx = -1;
            for (int i = 0; i < segment.Length; i++)
            {
                if (oldSegment.name.Contains(segment[i].name))
                {
                    prefabIdx = i;
                    break;
                }
            }
            if (prefabIdx < 0) prefabIdx = 0; // fallback

            segmentPool.Enqueue((oldSegment, prefabIdx));
        }
    }

    // Troca o segmento para um novo prefab (ex: ao apanhar ingrediente)
    public void SetSegmentOnly(GameObject newSegment)
    {
        segment = new GameObject[] { newSegment };
        // Limpa o pool para evitar respawn de segmentos antigos
        segmentPool.Clear();
    }

    // Troca para o segmento do índice desejado (0 a 6)
    public void SetSegmentIndex(int idx)
    {
        if (allSegments != null && idx >= 0 && idx < allSegments.Length)
        {
            segment = new GameObject[] { allSegments[idx] };
            segmentPool.Clear();
            currentSegmentIdx = idx;
            // Desativa todos os segmentos ativos antigos
            while (activeSegments.Count > 0)
            {
                var seg = activeSegments.Dequeue();
                if (seg != null)
                    seg.SetActive(false);
            }
            lastSegment = null;
            // Gera os segmentos iniciais do novo mapa
            for (int i = 0; i < maxSegments; i++)
            {
                SpawnSegment();
            }
        }
    }

    // Permite acessar os segmentos ativos como array (para reposicionar o player corretamente)
    public GameObject[] GetActiveSegmentsArray()
    {
        return activeSegments.ToArray();
    }
}
public class GameManager : MonoBehaviour
{
    public Segments segments;
    public Transform playerTransform;
    private int novoIndice = 0;

    void Update()
    {
        // Exemplo de troca de segmento ao pressionar a tecla "T"
        if (Input.GetKeyDown(KeyCode.T))
        {
            TrocarSegmento();
        }
    }

    void TrocarSegmento()
    {
        novoIndice++;
        if (novoIndice >= 7) novoIndice = 0; // Volta para 0 ao ultrapassar o índice 6

        // Após trocar o segmento
        segments.SetSegmentIndex(novoIndice);

        // Reposiciona o player para o início do novo segmento
        if (segments != null && playerTransform != null)
        {
            // Pega o primeiro segmento ativo após a troca
            GameObject[] ativos = segments.GetActiveSegmentsArray();
            if (ativos.Length > 0 && ativos[0] != null)
            {
                Vector3 pos = ativos[0].transform.position;
                // Mantém a altura original do player
                pos.y = playerTransform.position.y;
                playerTransform.position = pos;
            }
            else
            {
                playerTransform.position = Vector3.zero;
            }
        }
    }
}
