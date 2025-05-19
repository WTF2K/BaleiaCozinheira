using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -10f);
    public float followSpeed = 5f;

    private Vector3 fixedXY; // Guarda a posi��o fixa em X e Y

    void Start()
    {
        // Salva a posi��o inicial da c�mera em X e Y
        fixedXY = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mant�m X e Y fixos, s� segue o Z do peixe
        float targetZ = target.position.z + offset.z;
        Vector3 targetPosition = new Vector3(fixedXY.x + offset.x, fixedXY.y + offset.y, targetZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
