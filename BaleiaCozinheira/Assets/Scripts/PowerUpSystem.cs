using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerUpSystem : MonoBehaviour
{
    public GameObject shieldVisual;
    public float shieldDuration = 5f;
    public float turboDuration = 3f;
    public float turboMultiplier = 2f;
    public float radarDuration = 5f;
    public float radarRange = 15f;
    public LayerMask ingredientLayer;
    public Material highlightMaterial;

    private bool shieldActive = false;
    private bool turboActive = false;
    private bool radarActive = false;
    private float originalSpeed;
    private BaleiaSeguirRato whaleMovement;
    private PowerUpsManager PowerUpsManager;

    void Start()
    {
        whaleMovement = GetComponent<BaleiaSeguirRato>();
        originalSpeed = whaleMovement.forwardSpeed;

        shieldVisual.SetActive(false);
        PowerUpsManager = FindFirstObjectByType<PowerUpsManager>();

        if (PowerUpsManager == null)
            Debug.LogError("[PowerUpSystem] PowerUpsManager não encontrado!");
        else
            Debug.Log("[PowerUpSystem] PowerUpsManager encontrado!");
    }

    public void ActivateShield()
    {
        Debug.Log("[PowerUpSystem] ActivateShield chamado!");
        if (shieldActive) return;
        StartCoroutine(ShieldRoutine());
    }

    public void ActivateTurbo()
    {
        Debug.Log("[PowerUpSystem] ActivateTurbo chamado!");
        if (turboActive) return;
        StartCoroutine(TurboRoutine());
    }

    public void ActivateRadar()
    {
        Debug.Log("[PowerUpSystem] ActivateRadar chamado!");
        if (radarActive) return;
        StartCoroutine(RadarRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        // Desabilita colisões entre a layer do player, inimigos e tinta
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int inkLayer = LayerMask.NameToLayer("Ink");
        
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        Physics.IgnoreLayerCollision(playerLayer, inkLayer, true);

        shieldActive = true;
        shieldVisual.SetActive(true);
        if (PowerUpsManager != null)
        {
            PowerUpsManager.ShowPowerUpIcon(PowerUpType.Shield, true);
            StartCoroutine(PowerUpFade(PowerUpsManager.shieldIcon, shieldDuration));
        }

        whaleMovement.SetShield(true);
        yield return new WaitForSeconds(shieldDuration);

        // Reativa colisões após o fim do escudo
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        Physics.IgnoreLayerCollision(playerLayer, inkLayer, false);

        whaleMovement.SetShield(false);
        shieldVisual.SetActive(false);
        if (PowerUpsManager != null)
            PowerUpsManager.ShowPowerUpIcon(PowerUpType.Shield, false);

        shieldActive = false;
    }

    IEnumerator TurboRoutine()
    {
        turboActive = true;
        if (PowerUpsManager != null)
        {
            PowerUpsManager.ShowPowerUpIcon(PowerUpType.Turbo, true);
            StartCoroutine(PowerUpFade(PowerUpsManager.turboIcon, turboDuration));
        }

        whaleMovement.forwardSpeed *= turboMultiplier;
        yield return new WaitForSeconds(turboDuration);
        whaleMovement.forwardSpeed = originalSpeed;

        if (PowerUpsManager != null)
            PowerUpsManager.ShowPowerUpIcon(PowerUpType.Turbo, false);

        turboActive = false;
    }

    IEnumerator RadarRoutine()
    {
        radarActive = true;
        if (PowerUpsManager != null)
        {
            PowerUpsManager.ShowPowerUpIcon(PowerUpType.Radar, true);
            StartCoroutine(PowerUpFade(PowerUpsManager.radarIcon, radarDuration));
        }

        Collider[] ingredients = Physics.OverlapSphere(transform.position, radarRange, ingredientLayer);
        foreach (var ingredient in ingredients)
        {
            var renderer = ingredient.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material = highlightMaterial;
        }

        yield return new WaitForSeconds(radarDuration);

        foreach (var ingredient in ingredients)
        {
            var ingredientScript = ingredient.GetComponent<Ingredient>();
            if (ingredientScript != null)
                ingredientScript.ResetMaterial();
        }

        if (PowerUpsManager != null)
            PowerUpsManager.ShowPowerUpIcon(PowerUpType.Radar, false);

        radarActive = false;
    }

    // Lógica de fade de branco para preto com alpha 130 (0.51f)
    IEnumerator PowerUpFade(Image iconImage, float duration)
    {
        Color startColor = new Color(1f, 1f, 1f, 0.51f); // Branco translúcido
        Color endColor = new Color(0f, 0f, 0f, 0.51f);   // Preto translúcido

        float t = 0f;
        while (t < duration)
        {
            float lerpAmount = t / duration;
            iconImage.color = Color.Lerp(startColor, endColor, lerpAmount);
            t += Time.deltaTime;
            yield return null;
        }

        iconImage.color = endColor;
    }
}
