using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverPanel;


    public Segments segments;
    public Transform playerTransform;
    private int novoIndice = 0;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


    /*
    void Update()
    {
        // Exemplo de troca de segmento ao pressionar a tecla "T"
        if (Input.GetKeyDown(KeyCode.T))
        {
            TrocarSegmento();
        }
    }

    void TrocarSegmento()
    {
        novoIndice++;
        if (novoIndice >= 7) novoIndice = 0; // Volta para 0 ao ultrapassar o índice 6

        // Após trocar o segmento
        segments.SetSegmentIndex(novoIndice);

        // Reposiciona o player para o início do novo segmento
        if (segments != null && playerTransform != null)
        {
            // Pega o primeiro segmento ativo após a troca
            GameObject[] ativos = segments.GetActiveSegmentsArray();
            if (ativos.Length > 0 && ativos[0] != null)
            {
                Vector3 pos = ativos[0].transform.position;
                // Mantém a altura original do player
                pos.y = playerTransform.position.y;
                playerTransform.position = pos;
            }
            else
            {
                playerTransform.position = Vector3.zero;
            }
        }
    }*/
}
