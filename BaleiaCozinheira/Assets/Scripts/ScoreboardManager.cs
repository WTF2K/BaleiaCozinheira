using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScoreboardManager : MonoBehaviour
{
    public Transform scoresContainer;
    public GameObject scoreEntryPrefab;

    private class ScoreEntry
    {
        public string playerName;
        public int ingredients;
        public float distance;
        public int coins;
    }

    void Start()
    {
        MostrarPontuacoesOrdenadas();
    }

    void MostrarPontuacoesOrdenadas()
    {
        List<ScoreEntry> scores = new List<ScoreEntry>();

        for (int i = 0; i < 10; i++)
        {
            string key = $"Top{i}";
            if (PlayerPrefs.HasKey(key))
            {
                string[] parts = PlayerPrefs.GetString(key).Split('|');
                if (parts.Length == 4)
                {
                    scores.Add(new ScoreEntry
                    {
                        playerName = parts[0],
                        ingredients = int.Parse(parts[1]),
                        distance = float.Parse(parts[2]),
                        coins = int.Parse(parts[3])
                    });
                }
            }
        }

        // Ordenação personalizada: Ingredientes (desc), Distância (asc), Moedas (desc)
        var scoresOrdenados = scores
            .OrderByDescending(s => s.ingredients)
            .ThenBy(s => s.distance)
            .ThenByDescending(s => s.coins)
            .Take(10)
            .ToList();

        for (int i = 0; i < scoresOrdenados.Count; i++)
        {
            var s = scoresOrdenados[i];
            GameObject entry = Instantiate(scoreEntryPrefab, scoresContainer);
            TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 4)
            {
                texts[0].text = $"{i + 1}. {s.playerName}";
                texts[1].text = $"{s.ingredients} ingredientes";
                texts[2].text = $"{s.distance:000000}m";
                texts[3].text = $"{s.coins} moedas";
            }
        }
    }

    public void VoltarMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
