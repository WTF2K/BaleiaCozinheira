using UnityEngine;

public class BaleiaSeguirRato : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float forwardSpeed = 2f;
<<<<<<< Updated upstream
    public float depth = 10f;
    public float deadZoneDistance = 0.01f;

    [Range(0f, 0.4f)]
    public float protectionMargin = 0.05f; // margem da viewport (5% padrão)

    private Vector3 targetPos;

    void Start()
    {
        // Esconde o cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None; // Se quiser bloquear ao centro: CursorLockMode.Locked
    }
=======
    public float depthOffset = 10f; // dist�ncia da baleia � frente da c�mara

    private Vector3 targetWorldPos;
    private bool shieldActive = false;
>>>>>>> Stashed changes

    void Update()
    {
        if (Camera.main == null) return;

<<<<<<< Updated upstream
        // Rato → posição normalizada da viewport
        Vector3 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
=======
        // Calcula um plano de movimento � frente da c�mara
        Vector3 planeCenter = cameraReference.transform.position + cameraReference.transform.forward * depthOffset;
        Plane movementPlane = new Plane(-cameraReference.transform.forward, planeCenter);
>>>>>>> Stashed changes

        // Limitar com margem configurável
        float min = protectionMargin;
        float max = 1f - protectionMargin;

<<<<<<< Updated upstream
        mouseViewport.x = Mathf.Clamp(mouseViewport.x, min, max);
        mouseViewport.y = Mathf.Clamp(mouseViewport.y, min, max);
        mouseViewport.z = depth;
=======
        // Interse��o com o plano
        if (movementPlane.Raycast(ray, out float distance))
        {
            targetWorldPos = ray.GetPoint(distance);
        }
>>>>>>> Stashed changes

        // Viewport → mundo
        targetPos = Camera.main.ViewportToWorldPoint(mouseViewport);
        targetPos.z = transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 toTarget = targetPos - transform.position;
        Vector3 move = Vector3.zero;

<<<<<<< Updated upstream
        if (toTarget.magnitude > deadZoneDistance)
        {
            Vector3 direction = toTarget.normalized;
            move = direction * moveSpeed * Time.fixedDeltaTime;
        }
=======
        // Lerp at� � posi��o desejada (X/Y)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetWorldPos, followSpeed * Time.fixedDeltaTime);
>>>>>>> Stashed changes

        move.z = forwardSpeed * Time.fixedDeltaTime;
        transform.position += move;
    }
}
