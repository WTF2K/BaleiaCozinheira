using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    // Referências para os scripts que serão afetados pela dificuldade
    public GameObject player;
    public GameObject ingredientSpawner;
    public GameObject enemySpawner;
    public GameObject powerUpSpawner;

    // Configurações de dificuldade
    private int currentDifficulty = 0;
    private int maxDifficulty = 6;
    private int[] ingredientsCollected = new int[7];

    // Fatores de ajuste de dificuldade
    [Header("Ajustes de Dificuldade")]
    [Tooltip("Velocidade adicional ao jogador por nível de dificuldade")]
    public float playerSpeedIncreasePerLevel = 0.5f;

    [Tooltip("Aumento da taxa de spawn de inimigos por nível")]
    public float enemySpawnRateIncreasePerLevel = 0.2f;

    [Tooltip("Atraso adicional para spawn de power-ups por nível")]
    public float powerUpDelayIncreasePerLevel = 0.5f;

    // Valores base (iniciais)
    private float basePlayerSpeed;
    private float baseEnemySpawnRate;
    private float basePowerUpDelay;

    // Componentes
    private CharacterController playerController;
    private MonoBehaviour enemySpawnerScript;
    private MonoBehaviour powerUpSpawnerScript;

    // Dificuldade Normalizada
    public float NormalizedDifficulty { get; private set; } // 0→1 ao longo do jogo
    private float startTime;
    private float totalRampTime = 300f; // Tempo total para a ramp-up da dificuldade (ex: 5 minutos)

    void Awake() 
    { 
        Instance = this; 
        
        // Inicializar valores base
        if (player != null)
        {
            // Pega o forwardSpeed atual do BaleiaSeguirRato
            var baleiaScript = player.GetComponent<BaleiaSeguirRato>();
            if (baleiaScript != null)
            {
                basePlayerSpeed = baleiaScript.forwardSpeed;
            }
            else
            {
                basePlayerSpeed = 5.0f; // Valor padrão caso não encontre o componente
            }
        }

        // Inicializar valores do spawner de inimigos e power-ups
        baseEnemySpawnRate = 2.0f; // Valor padrão, ajuste conforme necessário
        basePowerUpDelay = 10.0f; // Valor padrão, ajuste conforme necessário

        startTime = Time.time;
    }

    void Update()
    {
        // atualiza NormalizedDifficulty entre 0 e 1
        NormalizedDifficulty = Mathf.Clamp01((Time.time - startTime) / totalRampTime);

        // Atualiza o nível de dificuldade com base nos ingredientes coletados
        UpdateDifficulty();
    }

    // Método chamado quando um ingrediente é coletado
    public void OnIngredientCollected(int ingredientType)
    {
        if (ingredientType >= 0 && ingredientType <= 6)
        {
            ingredientsCollected[ingredientType]++;
            
            // Destroi todos os inimigos, power-ups e coins existentes
            ClearExistingObjects();
            
            UpdateDifficulty();
        }
    }

    // Atualiza o nível de dificuldade com base nos ingredientes coletados
    private void UpdateDifficulty()
    {
        int uniqueIngredientsCollected = 0;
        
        // Conta quantos tipos diferentes de ingredientes foram coletados
        for (int i = 0; i < ingredientsCollected.Length; i++)
        {
            if (ingredientsCollected[i] > 0)
            {
                uniqueIngredientsCollected++;
            }
        }

        // Calcula a nova dificuldade (0 a 6)
        int newDifficulty = Mathf.Min(uniqueIngredientsCollected, maxDifficulty);
        
        // Se a dificuldade mudou, aplica os novos ajustes
        if (newDifficulty != currentDifficulty)
        {
            currentDifficulty = newDifficulty;
            ApplyDifficultySettings();
        }
    }

    // Aplica as configurações de dificuldade aos componentes do jogo
    private void ApplyDifficultySettings()
    {
        Debug.Log("Dificuldade aumentada para nível " + currentDifficulty);

        // Ajusta o forwardSpeed do jogador
        if (player != null)
        {
            // Acessa o forwardSpeed do BaleiaSeguirRato
            var baleiaScript = player.GetComponent<BaleiaSeguirRato>();
            if (baleiaScript != null)
            {
                float newForwardSpeed = basePlayerSpeed + (currentDifficulty * playerSpeedIncreasePerLevel);
                baleiaScript.forwardSpeed = newForwardSpeed;
                Debug.Log("Novo forwardSpeed da baleia: " + newForwardSpeed);
            }
        }

        // Ajusta a taxa de spawn de inimigos
        if (enemySpawner != null)
        {
            // Aqui você deve acessar a variável de taxa de spawn do seu spawner de inimigos
            // Por exemplo: enemySpawner.GetComponent<EnemySpawner>().spawnRate = baseEnemySpawnRate - (currentDifficulty * enemySpawnRateIncreasePerLevel);
            // Ajuste para o componente e variável corretos do seu jogo
            float newEnemyRate = Mathf.Max(0.5f, baseEnemySpawnRate - (currentDifficulty * enemySpawnRateIncreasePerLevel));
            Debug.Log("Nova taxa de spawn de inimigos: " + newEnemyRate);
        }

        // Ajusta o delay de power-ups
        if (powerUpSpawner != null)
        {
            // Aqui você deve acessar a variável de delay do seu spawner de power-ups
            // Por exemplo: powerUpSpawner.GetComponent<PowerUpSpawner>().spawnDelay = basePowerUpDelay + (currentDifficulty * powerUpDelayIncreasePerLevel);
            // Ajuste para o componente e variável corretos do seu jogo
            float newPowerUpDelay = basePowerUpDelay + (currentDifficulty * powerUpDelayIncreasePerLevel);
            Debug.Log("Novo delay de spawn de power-ups: " + newPowerUpDelay);
        }
    }

    // Para teste e depuração
    public string GetDifficultyStatus()
    {
        string status = "Nível de dificuldade: " + currentDifficulty + "\n";
        status += "Ingredientes coletados:\n";
        
        for (int i = 0; i < ingredientsCollected.Length; i++)
        {
            status += "Tipo " + i + ": " + ingredientsCollected[i] + "\n";
        }
        
        return status;
    }

    // Método para limpar objetos existentes no jogo
    private void ClearExistingObjects()
    {
        // Destroi todos os inimigos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
        // Destroi todos os power-ups
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerUp in powerUps)
        {
            Destroy(powerUp);
        }
        
        // Destroi todas as coins
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins)
        {
            Destroy(coin);
        }
        
        Debug.Log("Todos os inimigos, power-ups e coins foram destruídos.");
    }
}
