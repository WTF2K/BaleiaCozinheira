using UnityEngine;

public class BaleiaSeguirRato : MonoBehaviour
{
    public Camera cameraReference;
    public float followSpeed = 5f;
    public float forwardSpeed = 2f;
    public float depthOffset = 10f; // distância da baleia à frente da câmara

    private Vector3 targetWorldPos;

    void Update()
    {
        if (cameraReference == null) return;

        // Calcula um plano de movimento à frente da câmara
        Vector3 planeCenter = cameraReference.transform.position + cameraReference.transform.forward * depthOffset;
        Plane movementPlane = new Plane(-cameraReference.transform.forward, planeCenter);

        // Cria raio do rato
        Ray ray = cameraReference.ScreenPointToRay(Input.mousePosition);

        // Interseção com o plano
        if (movementPlane.Raycast(ray, out float distance))
        {
            targetWorldPos = ray.GetPoint(distance);
        }

        // Viewport em coordenadas do mundo no plano de movimento
        Vector3 worldMin = cameraReference.ViewportToWorldPoint(new Vector3(0f, 0f, depthOffset));
        Vector3 worldMax = cameraReference.ViewportToWorldPoint(new Vector3(1f, 1f, depthOffset));

        // Limitar X e Y ao viewport atual
        targetWorldPos.x = Mathf.Clamp(targetWorldPos.x, worldMin.x, worldMax.x);
        targetWorldPos.y = Mathf.Clamp(targetWorldPos.y, worldMin.y, worldMax.y);
        targetWorldPos.z = transform.position.z; // manter o Z atual da baleia
    }

    void FixedUpdate()
    {
        // Movimento para a frente (em Z)
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.fixedDeltaTime;

        // Lerp até à posição desejada (X/Y)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetWorldPos, followSpeed * Time.fixedDeltaTime);

        transform.position = smoothedPosition + forwardMove;
    }
}
