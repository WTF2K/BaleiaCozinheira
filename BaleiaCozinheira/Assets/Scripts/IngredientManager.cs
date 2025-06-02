using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    [Header("Imagem atual a apanhar")]
    public Image currentIngredientImg;
    public TextMeshProUGUI mapNameTXT;
    public TextMeshProUGUI mapTXT;

    [Header("Slots para ingredientes apanhados")]
    public Image[] collectedIngredientSlots;

    [Header("Dados dos ingredientes")]
    public Sprite[] ingredientSprites;
    public string[] ingredientNames;
    public string[] ingredientDescriptions;

    private int collectedCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Atualiza o ingrediente que está atualmente disponível para apanhar
    public void AtualizarIngredienteAtual(int index)
    {
        if (index < 0 || index >= ingredientSprites.Length) return;

        if (currentIngredientImg != null)
        {
            currentIngredientImg.sprite = ingredientSprites[index];
            currentIngredientImg.color = new Color(1f, 1f, 1f, 1f); // começa escurecido
        }

        if (mapNameTXT != null)
            mapNameTXT.text = ingredientNames[index];

        if (mapTXT != null)
            mapTXT.text = ingredientDescriptions[index];
    }

    // Atualiza o HUD ao apanhar um ingrediente
    public void RegistarIngredienteApanhado(int index)
    {
        if (collectedCount < collectedIngredientSlots.Length && index < ingredientSprites.Length)
        {
            Image slot = collectedIngredientSlots[collectedCount];
            slot.sprite = ingredientSprites[index];
            slot.color = new Color(1f, 1f, 1f, 1f); // branco visível
            collectedCount++;
        }

        if (currentIngredientImg != null)
            currentIngredientImg.color = new Color(1f, 1f, 1f, 1f); // mostra imagem atual como visível

        PolvoSeguidor polvo = FindObjectOfType<PolvoSeguidor>();
        if (polvo != null)
        {
            polvo.RecuarParaTrasDaCamera();
        }

        // ⚠️ Verifica se já foram apanhados todos
        if (collectedCount >= 7)
        {
            // Verificações de segurança
            if (IngredientManager.Instance == null || DistanceTracker.Instance == null || CoinManager.Instance == null)
            {
                Debug.LogWarning("Alguma instância está null!");
                return;
            }

            // Obter dados
            int ingredientes = IngredientManager.Instance.GetIngredientesApanhados();
            int distancia = DistanceTracker.Instance.GetDistance();
            int moedas = CoinManager.Instance.GetCoinCount();

            // Guardar no PlayerPrefs
            PlayerPrefs.SetInt("GameOver_Ingredients", ingredientes);
            PlayerPrefs.SetInt("GameOver_Distance", distancia);
            PlayerPrefs.SetInt("GameOver_Coins", moedas);
            PlayerPrefs.Save();


            Debug.Log("Todos os ingredientes apanhados! A mudar para a cena BossFight...");
            SceneManager.LoadScene("BossFight");
        }
    }

    public int GetIngredientesApanhados()
    {
        return collectedCount;
    }
}
