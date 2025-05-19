using UnityEngine;
using System.Collections;

public class PowerUpSystem : MonoBehaviour
{
    public GameObject shieldVisual; // Visual do escudo (Peixe Bola)
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
    private UIManager uiManager;

    void Start()
    {
        whaleMovement = GetComponent<BaleiaSeguirRato>();
            originalSpeed = whaleMovement.forwardSpeed;
        shieldVisual.SetActive(false);
        uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager == null)
            Debug.LogError("[PowerUpSystem] UIManager não encontrado!");
        else
            Debug.Log("[PowerUpSystem] UIManager encontrado!");
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
        shieldActive = true;
        shieldVisual.SetActive(true);
        if (uiManager != null) uiManager.ShowPowerUpIcon(PowerUpType.Shield, true);
        whaleMovement.SetShield(true);
        yield return new WaitForSeconds(shieldDuration);
        whaleMovement.SetShield(false);
        shieldVisual.SetActive(false);
        if (uiManager != null) uiManager.ShowPowerUpIcon(PowerUpType.Shield, false);
        shieldActive = false;
    }

    IEnumerator TurboRoutine()
    {
        turboActive = true;
        if (uiManager != null) uiManager.ShowPowerUpIcon(PowerUpType.Turbo, true);
        whaleMovement.forwardSpeed *= turboMultiplier;
        yield return new WaitForSeconds(turboDuration);
        whaleMovement.forwardSpeed = originalSpeed;
        if (uiManager != null) uiManager.ShowPowerUpIcon(PowerUpType.Turbo, false);
        turboActive = false;
    }

    IEnumerator RadarRoutine()
    {
        radarActive = true;
        if (uiManager != null) uiManager.ShowPowerUpIcon(PowerUpType.Radar, true);
        Collider[] ingredients = Physics.OverlapSphere(transform.position, radarRange, ingredientLayer);
        foreach (var ingredient in ingredients)
        {
            var renderer = ingredient.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = highlightMaterial;
            }
        }
        yield return new WaitForSeconds(radarDuration);
        foreach (var ingredient in ingredients)
        {
            var ingredientScript = ingredient.GetComponent<Ingredient>();
            if (ingredientScript != null)
            {
                ingredientScript.ResetMaterial();
            }
        }
        if (uiManager != null) uiManager.ShowPowerUpIcon(PowerUpType.Radar, false);
        radarActive = false;
    }
}