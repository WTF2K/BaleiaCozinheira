using UnityEngine;

public class BaleiaSeguirRato : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float forwardSpeed = 2f;
    public float depth = 10f;
    public float deadZoneDistance = 0.01f;

    [Range(0f, 0.4f)]
    public float protectionMargin = 0.05f; // margem da viewport (5% padrão)

    private Vector3 targetPos;
    private bool shieldActive = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Camera.main == null) return;

        Vector3 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float min = protectionMargin;
        float max = 1f - protectionMargin;

        mouseViewport.x = Mathf.Clamp(mouseViewport.x, min, max);
        mouseViewport.y = Mathf.Clamp(mouseViewport.y, min, max);
        mouseViewport.z = depth;

        targetPos = Camera.main.ViewportToWorldPoint(mouseViewport);
        targetPos.z = transform.position.z;

        float lerpSpeed = moveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);

        if (forwardSpeed != 0f)
        {
            transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        }
    }

    public void SetShield(bool active)
    {
        shieldActive = active;
    }

    // Novo método para reduzir velocidade
    public void ReduceSpeed(float amount)
    {
        if (shieldActive) return; // se estiver protegido, não perde velocidade

        moveSpeed = Mathf.Max(0f, moveSpeed - amount);
        forwardSpeed = Mathf.Max(0f, forwardSpeed - amount);
    }
}
