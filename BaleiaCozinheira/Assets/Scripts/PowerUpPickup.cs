using UnityEngine;

public enum PowerUpType { Shield, Turbo, Radar }

public class PowerUpPickup : MonoBehaviour
{
    public PowerUpType powerUpType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[PowerUpPickup] Triggered by: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("[PowerUpPickup] Player detected!");

            PowerUpSystem powerUpSystem = other.GetComponent<PowerUpSystem>();
            if (powerUpSystem != null)
            {
                Debug.Log($"[PowerUpPickup] Activating power up: {powerUpType}");
                switch (powerUpType)
                {
                    case PowerUpType.Shield:
                        powerUpSystem.ActivateShield();
                        break;
                    case PowerUpType.Turbo:
                        powerUpSystem.ActivateTurbo();
                        break;
                    case PowerUpType.Radar:
                        powerUpSystem.ActivateRadar();
                        break;
                }
            }
            else
            {
                Debug.LogWarning("[PowerUpPickup] PowerUpSystem component not found on player.");
            }
            Destroy(gameObject); // Remove o power-up da cena
        }
    }
}