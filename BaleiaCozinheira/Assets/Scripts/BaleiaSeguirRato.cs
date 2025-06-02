using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaleiaSeguirRato : MonoBehaviour
{
    [Header("Velocidade")]
    public float moveSpeed = 2f;
    public float forwardSpeed = 2f;
    private float originalMoveSpeed;
    private float originalForwardSpeed;
    public float recoveryRate = 0.5f; // Velocidade de recuperação por segundo

    [Header("Posição do rato")]
    public float depth = 10f;
    public float deadZoneDistance = 0.01f;
    [Range(0f, 0.4f)]
    public float protectionMargin = 0.05f;

    [Header("Colisões e escudo")]
    private bool shieldActive = false;
    public float collisionThreshold = 2f;
    public int maxHitsWithinThreshold = 3;
    private List<float> collisionTimes = new List<float>();
    public float collisionCooldown = 1f;
    private float lastCollisionTime = -Mathf.Infinity;

    [Header("Efeito de piscar")]
    public float blinkDuration = 1f;
    public int blinkCount = 5;

    private Vector3 targetPos;

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

        // Limita o rato à viewport
        Vector3 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float min = protectionMargin;
        float max = 1f - protectionMargin;
        mouseViewport.x = Mathf.Clamp(mouseViewport.x, min, max);
        mouseViewport.y = Mathf.Clamp(mouseViewport.y, min, max);
        mouseViewport.z = depth;

        // Converter para posição no mundo
        targetPos = Camera.main.ViewportToWorldPoint(mouseViewport);
        targetPos.z = transform.position.z;

        // Movimento lateral
        float lerpSpeed = moveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);

        // Movimento para a frente
        if (forwardSpeed != 0f)
        {
            transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        }

        // ✅ Recupera sempre a velocidade, com ou sem escudo
        moveSpeed = Mathf.MoveTowards(moveSpeed, originalMoveSpeed, recoveryRate * Time.deltaTime);
        forwardSpeed = Mathf.MoveTowards(forwardSpeed, originalForwardSpeed, recoveryRate * Time.deltaTime);
    }

    public void SetShield(bool active)
    {
        shieldActive = active;
    }

    public void ReduceSpeed(float amount)
    {
        if (shieldActive)
            return; // 🛡️ Não reduz velocidade se o escudo estiver ativo

        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        lastCollisionTime = Time.time;

        float reduction = amount * 2f;
        moveSpeed = Mathf.Max(0f, moveSpeed - reduction);
        forwardSpeed = Mathf.Max(0f, forwardSpeed - reduction);

        // Regista a colisão
        collisionTimes.Add(Time.time);
        collisionTimes.RemoveAll(t => Time.time - t > collisionThreshold);

        Debug.Log("Colisão com inimigo! Contagem recente: " + collisionTimes.Count);

        StartCoroutine(BlinkEffect(blinkDuration, blinkCount));

        if (collisionTimes.Count >= maxHitsWithinThreshold)
        {
            GameOver();
        }
    }

    IEnumerator BlinkEffect(float duration, int count)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
            yield break;

        float halfPeriod = duration / (count * 2);
        for (int i = 0; i < count; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(halfPeriod);
            rend.enabled = true;
            yield return new WaitForSeconds(halfPeriod);
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER - Demasiadas colisões!");
        SceneManager.LoadScene("GameOver");
    }
}
