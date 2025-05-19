using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image shieldIcon;
    public Image turboIcon;
    public Image radarIcon;

    void Start()
    {
        shieldIcon.enabled = false;
        turboIcon.enabled = false;
        radarIcon.enabled = false;
    }

    public void ShowPowerUpIcon(PowerUpType type, bool show)
    {
        Debug.Log($"[UIManager] ShowPowerUpIcon: {type}, mostrar: {show}");
        switch (type)
        {
            case PowerUpType.Shield:
                shieldIcon.enabled = show;
                break;
            case PowerUpType.Turbo:
                turboIcon.enabled = show;
                break;
            case PowerUpType.Radar:
                radarIcon.enabled = show;
                break;
        }
    }
}