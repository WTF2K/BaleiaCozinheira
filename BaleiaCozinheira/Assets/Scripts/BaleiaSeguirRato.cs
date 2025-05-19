using UnityEngine;

public class BaleiaSeguirRato : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float forwardSpeed = 2f;
    public float depth = 10f;
    public float deadZoneDistance = 0.01f; // zona morta para evitar tremor

    private Vector3 targetPos;

    void Update()
    {
        if (Camera.main == null) return;

        // Rato → viewport (limitado)
        Vector3 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseViewport.x = Mathf.Clamp01(mouseViewport.x);
        mouseViewport.y = Mathf.Clamp01(mouseViewport.y);
        mouseViewport.z = depth;

        // Viewport → posição no mundo
        targetPos = Camera.main.ViewportToWorldPoint(mouseViewport);
        targetPos.z = transform.position.z; // manter o Z atual
    }

    void FixedUpdate()
    {
        // Calcular distância até ao alvo
        Vector3 toTarget = targetPos - transform.position;

        // Se for maior que a zona morta, aplica movimento lateral/vertical
        Vector3 move = Vector3.zero;

        if (toTarget.magnitude > deadZoneDistance)
        {
            Vector3 direction = toTarget.normalized;
            move = direction * moveSpeed * Time.fixedDeltaTime;
        }

        // Movimento constante para a frente (Z)
        move.z = forwardSpeed * Time.fixedDeltaTime;

        transform.position += move;
    }
}
