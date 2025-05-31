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

    // Atualiza o ingrediente que está atualmente disponível para apanhar
    public void AtualizarIngredienteAtual(int index)
    {
        if (index < 0 || index >= ingredientSprites.Length) return;

        // Atualizar imagem e nomes do ingrediente atual
        if (currentIngredientImg != null)
        {
            currentIngredientImg.sprite = ingredientSprites[index];
            currentIngredientImg.color = new Color(1f, 1f, 1f, 1f);
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
    }
}
