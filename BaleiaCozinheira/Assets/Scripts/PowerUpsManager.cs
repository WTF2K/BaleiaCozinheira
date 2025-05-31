using UnityEngine;
using UnityEngine.UI;

public class PowerUpsManager : MonoBehaviour
{
    public Image shieldIcon;
    public Image turboIcon;
    public Image radarIcon;

    public void ShowPowerUpIcon(PowerUpType type, bool show)
    {
        Debug.Log($"[UIManager] ShowPowerUpIcon: {type}, mostrar: {show}");
        Color activeColor = new Color(1f, 1f, 1f, 0.51f);
        Color inactiveColor = new Color(0f, 0f, 0f, 0.51f);

        switch (type)
        {
            case PowerUpType.Shield:
                shieldIcon.color = show ? activeColor : inactiveColor;
                break;
            case PowerUpType.Turbo:
                turboIcon.color = show ? activeColor : inactiveColor;
                break;
            case PowerUpType.Radar:
                radarIcon.color = show ? activeColor : inactiveColor;
                break;
        }
    }
}