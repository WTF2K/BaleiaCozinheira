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
    private GameObject[] allSegments;
    private int currentSegmentIdx = 0;
    public EnemySpawner1 enemySpawner;

    void Start()
    {
        allSegments = (GameObject[])segment.Clone();
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

        if (segmentPool.Count > 0)
        {
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

            int prefabIdx = -1;
            for (int i = 0; i < segment.Length; i++)
            {
                if (oldSegment.name.Contains(segment[i].name))
                {
                    prefabIdx = i;
                    break;
                }
            }
            if (prefabIdx < 0) prefabIdx = 0;

            segmentPool.Enqueue((oldSegment, prefabIdx));
        }
    }

    public void SetSegmentOnly(GameObject newSegment)
    {
        segment = new GameObject[] { newSegment };
        segmentPool.Clear();
    }

    public void SetSegmentIndex(int idx)
    {
        if (allSegments != null && idx >= 0 && idx < allSegments.Length)
        {
            segment = new GameObject[] { allSegments[idx] };
            segmentPool.Clear();
            currentSegmentIdx = idx;
            while (activeSegments.Count > 0)
            {
                var seg = activeSegments.Dequeue();
                if (seg != null)
                    seg.SetActive(false);
            }
            lastSegment = null;
            for (int i = 0; i < maxSegments; i++)
            {
                SpawnSegment();
            }
        }
        enemySpawner?.ResetSpawner();

    }

    public GameObject[] GetActiveSegmentsArray()
    {
        return activeSegments.ToArray();
    }
}


