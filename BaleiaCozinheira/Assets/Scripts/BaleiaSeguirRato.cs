using UnityEngine;

public class BaleiaSeguirRato : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float forwardSpeed = 2f;
    public float depth = 10f;
    public float deadZoneDistance = 0.01f;

    [Range(0f, 0.4f)]
    public float protectionMargin = 0.05f; // margem da viewport (5% padrão)

    private Vector3 targetPos;

    private bool shieldActive = false;

    void Start()
    {
        // Esconde o cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None; // Se quiser bloquear ao centro: CursorLockMode.Locked
    }

    void Update()
    {
        if (Camera.main == null) return;

        // Rato → posição normalizada da viewport
        Vector3 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Limitar com margem configurável
        float min = protectionMargin;
        float max = 1f - protectionMargin;

        mouseViewport.x = Mathf.Clamp(mouseViewport.x, min, max);
        mouseViewport.y = Mathf.Clamp(mouseViewport.y, min, max);
        mouseViewport.z = depth;

        // Viewport → mundo
        targetPos = Camera.main.ViewportToWorldPoint(mouseViewport);
        targetPos.z = transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 toTarget = targetPos - transform.position;
        Vector3 move = Vector3.zero;

        if (toTarget.magnitude > deadZoneDistance)
        {
            Vector3 direction = toTarget.normalized;
            move = direction * moveSpeed * Time.fixedDeltaTime;
        }

        move.z = forwardSpeed * Time.fixedDeltaTime;
        transform.position += move;
    }

    public void SetShield(bool active)
    {
        shieldActive = active;
    }

    // Depois, no teu código de colisão/dano, verifica shieldActive:
    // if (shieldActive) { /* ignora dano */ }
}
