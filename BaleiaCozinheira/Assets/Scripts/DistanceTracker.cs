using UnityEngine;
using TMPro;

public class DistanceTracker : MonoBehaviour
{
    public static DistanceTracker Instance;

    public TextMeshProUGUI distanceText;
    public Transform player;

    private float distance = 0f;
    private Vector3 lastPosition;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        lastPosition = player.position;
    }

    void Update()
    {
        float delta = Vector3.Distance(player.position, lastPosition);
        distance += delta;
        lastPosition = player.position;

        int displayDistance = Mathf.FloorToInt(distance);
        distanceText.text = displayDistance.ToString("D8");
    }

    public int GetDistance()
    {
        return Mathf.FloorToInt(distance);
    }
}
