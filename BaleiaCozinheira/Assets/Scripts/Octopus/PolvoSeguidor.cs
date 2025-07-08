using UnityEngine;
using UnityEngine.SceneManagement;

public class PolvoSeguidor : MonoBehaviour
{
    private bool ativo = false;
    private Transform alvo;
    private Vector3 offset;
    private Vector3 posicaoEscondida;

    public float velocidade = 5f; // Adiciona velocidade visível no inspector

    void Start()
    {
        alvo = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Verifica se a câmara tem tag "MainCamera"
        if (Camera.main != null)
        {
            posicaoEscondida = Camera.main.transform.position - Camera.main.transform.forward * 20f;
            posicaoEscondida.z -= 10f;
        }
        else
        {
            posicaoEscondida = new Vector3(0, -1000, 0); // fallback escondido
        }

        transform.position = posicaoEscondida;
        ativo = false;

        // ⚠️ NÃO usar SetActive(false)! Só esconder fora de viewport
    }

    void Update()
    {
        if (!ativo || alvo == null)
            return;

        Vector3 direction = alvo.position - transform.position;
        transform.position += direction.normalized * velocidade * Time.deltaTime;
    }

    public void AtivarPolvo(Transform alvoPlayer)
    {
        Debug.Log("AtivarPolvo foi chamado!");

        if (alvoPlayer == null)
        {
            Debug.LogError("O alvoPlayer é null!");
            return;
        }

        this.alvo = alvoPlayer;
        ativo = true;

        if (Camera.main != null)
        {
            // Coloca o polvo ATRÁS da câmara no eixo Z
            Vector3 atrasDaCamera = Camera.main.transform.position - Camera.main.transform.forward * 10f;
            atrasDaCamera.z = alvo.position.z - 20f; // bem atrás da baleia
            transform.position = atrasDaCamera;
        }

        Debug.Log("Polvo ativado atrás da câmara na posição: " + transform.position);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Polvo colidiu com a baleia! Ir para GameOver.");
            SceneManager.LoadScene("GameOver");
        }
    }

    public void EsconderPolvo()
    {
        ativo = false;
        transform.position = posicaoEscondida;
    }
}
