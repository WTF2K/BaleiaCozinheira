using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segements : MonoBehaviour
{
    public GameObject[] segment;
    [SerializeField] int xPos = 10;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] int segmentNum;

    void Update()
    {
        if (creatingSegment == false)
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        //segmentNum = Random.Range(0, 1);
        segmentNum = Random.Range(0, segment.Length);
        Instantiate(segment[segmentNum], new Vector3(xPos, 0, 0), Quaternion.identity);
        xPos += 10;
        yield return new WaitForSeconds(3);
        creatingSegment = false;
    }
}
