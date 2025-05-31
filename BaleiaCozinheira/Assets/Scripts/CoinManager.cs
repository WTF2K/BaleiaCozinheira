using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString("D6");
    }
}
