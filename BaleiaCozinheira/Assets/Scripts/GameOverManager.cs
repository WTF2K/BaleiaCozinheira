using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text ingredientsText;
    public TMP_Text distanceText;
    public TMP_Text coinsText;

    private int ingredients;
    private int distance;
    private int coins;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Obter os valores guardados pela pausa
        ingredients = PlayerPrefs.GetInt("GameOver_Ingredients", 0);
        distance = PlayerPrefs.GetInt("GameOver_Distance", 0);
        coins = PlayerPrefs.GetInt("GameOver_Coins", 0);

        ingredientsText.text = $"Ingredientes: {ingredients}";
        distanceText.text = $"Distância: {distance:D6}";
        coinsText.text = $"Moedas: {coins}";
    }

    public void GuardarResultado()
    {
        string nome = playerNameInput.text;
        if (string.IsNullOrEmpty(nome)) nome = "Anónimo";

        string novoResultado = $"{nome}|{ingredients}|{distance}|{coins}";

        // Guardar nos Top 10 (exemplo simples)
        for (int i = 0; i < 10; i++)
        {
            if (!PlayerPrefs.HasKey($"Top{i}"))
            {
                PlayerPrefs.SetString($"Top{i}", novoResultado);
                break;
            }
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene("Scoreboard");
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
