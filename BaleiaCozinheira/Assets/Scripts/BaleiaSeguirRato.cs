using System.Collections;
using System.Collections.Generic;
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

    // Tempo limite para considerar colisões consecutivas (2 segundos, por exemplo)
    public float collisionThreshold = 2f;
    private List<float> collisionTimes = new List<float>();

    // NOVO: Limite de colisões dentro do intervalo para Game Over
    public int maxHitsWithinThreshold = 2;

    // Parâmetros para o efeito de piscar
    public float blinkDuration = 1f;
    public int blinkCount = 5;

    // Variável para cooldown de colisão
    public float collisionCooldown = 1f;
    private float lastCollisionTime = -Mathf.Infinity;

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
        if (shieldActive)
            return;

        // Verifica se passou o cooldown de colisão
        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        lastCollisionTime = Time.time;

        // Multiplica o 'amount' por 2 para reduzir mais a velocidade
        float reduction = amount * 2f;
        moveSpeed = Mathf.Max(0f, moveSpeed - reduction);
        forwardSpeed = Mathf.Max(0f, forwardSpeed - reduction);

        // Registra o instante da colisão
        collisionTimes.Add(Time.time);
        // Remove colisões que ocorreram há mais de collisionThreshold segundos
        collisionTimes.RemoveAll(t => Time.time - t > collisionThreshold);

        Debug.Log("Colisão com inimigo! Contagem em " + collisionThreshold + "s: " + collisionTimes.Count);

        // Ativa o efeito de piscar
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
        Debug.Log("Game Over - " + maxHitsWithinThreshold + " colisões dentro de " + collisionThreshold + " segundos");
        SceneManager.LoadScene("GameOver");
    }
}
