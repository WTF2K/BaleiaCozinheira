using UnityEngine;

public class PolvoSeguidor : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float distanciaAtrasDaCamera = 150f;
    public float distanciaFrenteCamera = 1f;

    private bool perseguir = false;

    void Update()
    {
        if (player == null || cameraTransform == null) return;

        if (!perseguir)
        {
            // Fica atrás da câmara enquanto não está a perseguir
            Vector3 atrasDaCamera = cameraTransform.position - cameraTransform.forward * distanciaAtrasDaCamera;
            transform.position = Vector3.Lerp(transform.position, atrasDaCamera, Time.deltaTime * 2f);
        }
        else
        {
            // Persegue a baleia
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    public void AtivarPerseguicao()
    {
        // Já está a perseguir? Não fazer nada
        if (perseguir) return;

        if (cameraTransform == null || player == null)
        {
            Debug.LogWarning("Faltam referências no PolvoSeguidor!");
            return;
        }

        // Coloca à frente da câmara apenas na primeira ativação
        Vector3 frenteDaCamera = cameraTransform.position + cameraTransform.forward * distanciaFrenteCamera;
        frenteDaCamera.y = player.position.y;

        transform.position = frenteDaCamera;
        perseguir = true;

        Debug.Log("🐙 Polvo apareceu à frente da câmara e iniciou perseguição!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🐙 Polvo perseguidor colidiu com a baleia!");

            // Segurança: verifica se as instâncias existem
            if (IngredientManager.Instance == null || DistanceTracker.Instance == null || CoinManager.Instance == null)
            {
                Debug.LogWarning("⚠️ Uma das instâncias está null ao tentar guardar dados!");
                return;
            }

            // Guarda os dados no PlayerPrefs
            PlayerPrefs.SetInt("GameOver_Ingredients", IngredientManager.Instance.GetIngredientesApanhados());
            PlayerPrefs.SetInt("GameOver_Distance", DistanceTracker.Instance.GetDistance());
            PlayerPrefs.SetInt("GameOver_Coins", CoinManager.Instance.GetCoinCount());
            PlayerPrefs.Save();

            // Muda para a cena de Game Over
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }

    public void RecuarParaTrasDaCamera()
    {
        perseguir = false;
        transform.position = cameraTransform.position - cameraTransform.forward * distanciaAtrasDaCamera;
    }

    public bool IsFollowing()
    {
        return perseguir;
    }
}
