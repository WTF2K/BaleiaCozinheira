using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button continuarBtn, recome�arBtn, menuPrincipalBtn, desistirBtn;

    private bool jogoPausado = false;

    void Start()
    {
        pausePanel.SetActive(false);

        continuarBtn.onClick.AddListener(Continuar);
        recome�arBtn.onClick.AddListener(Recome�ar);
        menuPrincipalBtn.onClick.AddListener(MenuPrincipal);
        desistirBtn.onClick.AddListener(Desistir);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
                Continuar();
            else
                Pausar();
        }
    }

    void Pausar()
    {
        jogoPausado = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    void Continuar()
    {
        jogoPausado = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    void Recome�ar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal"); // substitui pelo nome correto da tua scene
    }

    public void Desistir()
    {
        // Verifica��es de seguran�a
        if (IngredientManager.Instance == null || DistanceTracker.Instance == null || CoinManager.Instance == null)
        {
            Debug.LogWarning("Alguma inst�ncia est� null!");
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

        Debug.Log($"[PauseManager] A guardar resultado: Ingredientes={ingredientes}, Dist�ncia={distancia}, Moedas={moedas}");

        // Ir para a cena GameOver
        SceneManager.LoadScene("GameOver");
    }
}
