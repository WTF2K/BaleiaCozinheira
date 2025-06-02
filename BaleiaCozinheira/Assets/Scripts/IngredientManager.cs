using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // Atualiza o ingrediente que est� atualmente dispon�vel para apanhar
    public void AtualizarIngredienteAtual(int index)
    {
        if (index < 0 || index >= ingredientSprites.Length) return;

        if (currentIngredientImg != null)
        {
            currentIngredientImg.sprite = ingredientSprites[index];
            currentIngredientImg.color = new Color(0f, 0f, 0f, 0.5f); // come�a escurecido
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
            slot.color = new Color(1f, 1f, 1f, 1f); // branco vis�vel
            collectedCount++;
        }

        if (currentIngredientImg != null)
            currentIngredientImg.color = new Color(1f, 1f, 1f, 1f); // mostra imagem atual como vis�vel
    }

    public int GetIngredientesApanhados()
    {
        return collectedCount;
    }
}
