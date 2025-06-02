using UnityEngine;
using UnityEngine.SceneManagement;

public class BaleiaSeguirRato : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float forwardSpeed = 2f;
    public float depth = 10f;
    public float deadZoneDistance = 0.01f;

    [Range(0f, 0.4f)]
    public float protectionMargin = 0.05f;

    private Vector3 targetPos;
    private bool shieldActive = false;

    private float originalMoveSpeed;
    private float originalForwardSpeed;
    public float recoveryRate = 0.05f;

    // NOVO: contagem de colisões
    private int hitCount = 0;
    public int maxHits = 3;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

        originalMoveSpeed = moveSpeed;
        originalForwardSpeed = forwardSpeed;
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

        if (!shieldActive)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, originalMoveSpeed, recoveryRate * Time.deltaTime);
            forwardSpeed = Mathf.MoveTowards(forwardSpeed, originalForwardSpeed, recoveryRate * Time.deltaTime);
        }
    }

    public void SetShield(bool active)
    {
        shieldActive = active;
    }

    public void ReduceSpeed(float amount)
    {
        if (shieldActive) return;

        moveSpeed = Mathf.Max(0f, moveSpeed - amount);
        forwardSpeed = Mathf.Max(0f, forwardSpeed - amount);

        // NOVO: contabilizar a colisão
        hitCount++;
        Debug.Log("Colisão com inimigo! Total: " + hitCount);

        if (hitCount >= maxHits)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over - 3 colisões");
        SceneManager.LoadScene("GameOver");
    }
}
