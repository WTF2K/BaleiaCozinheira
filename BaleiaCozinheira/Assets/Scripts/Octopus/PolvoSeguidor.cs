using UnityEngine;

public class PolvoSeguidor : MonoBehaviour
{
    private bool ativo = false;
    private Transform alvo;
    private Vector3 offset;
    private Vector3 posicaoEscondida;

    void Start()
    {
        // Encontra o jogador (baleia)
        alvo = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Define posição escondida atrás da câmara
        if (Camera.main != null)
        {
            posicaoEscondida = Camera.main.transform.position - Camera.main.transform.forward * 20f;
            posicaoEscondida.z -= 10f;
        }
        else
        {
            posicaoEscondida = new Vector3(0, -1000, 0); // fallback
        }

        EsconderPolvo(); // Começa escondido
    }

    void Update()
    {
        if (!ativo || alvo == null) return;

        Vector3 novaPosicao = alvo.position + offset;
        novaPosicao.z = alvo.position.z - 10f;
        transform.position = Vector3.Lerp(transform.position, novaPosicao, Time.deltaTime);
    }

    public void AtivarPolvo(Transform novoAlvo)
    {
        alvo = novoAlvo;
        offset = transform.position - alvo.position;
        ativo = true;
        gameObject.SetActive(true);
    }

    public void EsconderPolvo()
    {
        ativo = false;
        transform.position = posicaoEscondida;
        gameObject.SetActive(false);
    }
}
