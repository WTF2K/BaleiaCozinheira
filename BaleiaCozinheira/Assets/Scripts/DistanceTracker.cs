using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceTracker : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;
    private float startingZ;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player não atribuído ao DistanceTracker.");
            enabled = false;
            return;
        }

        startingZ = player.position.z;
    }

    void Update()
    {
        float distance = player.position.z - startingZ;
        distance = Mathf.Max(0, distance);

        // Mostra como 00000001, 00000002, etc.
        distanceText.text = Mathf.FloorToInt(distance).ToString("D8");
    }
}
