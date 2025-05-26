using UnityEngine;

public class PolvoInimigo : MonoBehaviour
{
    [Header("Patrulha")]
    public int numeroPontos = 5;
    public float areaX = 20f;
    public float areaY = 10f;
    public Transform centroPatrulha;

    [Header("Comportamento")]
    public Transform player;
    public float velocidade = 3f;
    public float distanciaParaPerseguir = 7f;
    public GameObject tintaPrefab;
    public float intervaloTinta = 2f;

    private Transform[] pontosPatrulha;
    private int indexAtual = 0;
    private bool perseguindo = false;
    private float tempoDesdeUltimaTinta = 0f;

    void Start()
    {
        GerarPontosPatrulha();
    }

    void Update()
    {
        if (player == null || pontosPatrulha.Length == 0) return;

        float distancia = Vector3.Distance(transform.position, player.position);
        perseguindo = distancia < distanciaParaPerseguir;

        if (perseguindo)
        {
            Perseguir();

            tempoDesdeUltimaTinta += Time.deltaTime;
            if (tempoDesdeUltimaTinta >= intervaloTinta)
            {
                LançarTinta();
                tempoDesdeUltimaTinta = 0f;
            }
        }
        else
        {
            Patrulhar();
        }

        if (perseguindo)
        {
            float distanciaAgarre = 1.5f;

            if (Vector3.Distance(transform.position, player.position) < distanciaAgarre)
            {
                Debug.Log("Polvo agarrou a baleia!");
                GameManager.Instance.GameOver();
            }
        }


    }

    void GerarPontosPatrulha()
    {
        pontosPatrulha = new Transform[numeroPontos];
        Vector3 centro = centroPatrulha != null ? centroPatrulha.position : transform.position;

        for (int i = 0; i < numeroPontos; i++)
        {
            Vector3 pos = centro + new Vector3(
                Random.Range(-areaX, areaX),
                Random.Range(-areaY, areaY),
                0f
            );

            GameObject ponto = new GameObject("PontoPatrulha_" + i);
            ponto.transform.position = pos;
            pontosPatrulha[i] = ponto.transform;
        }
    }

    public void AtualizarPontosDePatrulha(int novoNumero, float novaAreaX, float novaAreaY)
    {
        // Apaga os pontos antigos
        if (pontosPatrulha != null)
        {
            foreach (Transform t in pontosPatrulha)
            {
                if (t != null)
                    Destroy(t.gameObject);
            }
        }

        // Cria nova lista
        pontosPatrulha = new Transform[novoNumero];
        Vector3 centro = centroPatrulha != null ? centroPatrulha.position : transform.position;

        for (int i = 0; i < novoNumero; i++)
        {
            Vector3 pos = centro + new Vector3(
                Random.Range(-novaAreaX, novaAreaX),
                Random.Range(-novaAreaY, novaAreaY),
                0f
            );

            GameObject ponto = new GameObject("PontoPatrulha_" + i);
            ponto.transform.position = pos;
            pontosPatrulha[i] = ponto.transform;
        }
    }


    void Patrulhar()
    {
        Transform alvo = pontosPatrulha[indexAtual];
        MoverPara(alvo.position);

        if (Vector3.Distance(transform.position, alvo.position) < 0.3f)
        {
            indexAtual = (indexAtual + 1) % pontosPatrulha.Length;
        }
    }

    void Perseguir()
    {
        MoverPara(player.position);
    }

    void MoverPara(Vector3 destino)
    {
        Vector3 direcao = (destino - transform.position).normalized;
        transform.position += direcao * velocidade * Time.deltaTime;

        if (direcao != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direcao), 5f * Time.deltaTime);
    }

    void LançarTinta()
    {
        if (tintaPrefab != null)
        {
            Instantiate(tintaPrefab, transform.position - transform.forward, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        if (pontosPatrulha == null) return;

        Gizmos.color = Color.magenta;
        foreach (Transform t in pontosPatrulha)
        {
            if (t != null)
                Gizmos.DrawSphere(t.position, 0.5f);
        }
    }
    void OnDestroy()
    {
        foreach (Transform t in pontosPatrulha)
        {
            if (t != null)
                Destroy(t.gameObject);
        }
    }

    public void AparecerAtrasDoPlayer(Transform playerTransform)
    {
        Vector3 offset = -playerTransform.forward * 5f + Vector3.up * 1f;
        transform.position = playerTransform.position + offset;
        transform.LookAt(playerTransform);

        perseguindo = true;
        velocidade *= 1.5f;
        Debug.Log("Polvo apareceu atrás da baleia!");
    }

}
