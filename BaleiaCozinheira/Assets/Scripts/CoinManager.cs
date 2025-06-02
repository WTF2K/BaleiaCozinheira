using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("HUD")]
    public TextMeshProUGUI coinText;

    private int coinCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AtualizarHUD();
    }

    public void AddCoin()
    {
        coinCount++;
        AtualizarHUD();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        if (coinText != null)
        {
            coinText.text = coinCount.ToString("D4");
        }
    }

    private void AtualizarHUD()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString("00000000");
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void ResetCoins()
    {
        coinCount = 0;
        AtualizarHUD();
    }
}
