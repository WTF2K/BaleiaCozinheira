using System.Collections;
using UnityEngine;

public class PolvoFugaAposAparicao : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Camera cameraPrincipal;

    [Header("Aparição")]
    public float tempoVisivel = 5f;
    public float distanciaFrontal = 15f;
    public Vector3 offsetLocal = Vector3.zero;

    [Header("Intervalo Aleatório")]
    public float intervaloMinimo = 10f;
    public float intervaloMaximo = 20f;

    [Header("Fuga")]
    public float velocidadeFugaLateral = 5f;

    // Estado interno
    private float cronometro = 0f;
    private float proximoIntervalo = 0f;
    private float tempoAtivo = 0f;
    private bool ativo = false;
    private bool emFuga = false;
    private Vector3 direcaoFuga = Vector3.zero;

    [Header("Tinta")]
    public GameObject tintaPrefab;
    public int numeroJatos = 3;
    public float intervaloEntreJatos = 0.5f;

    void Start()
    {
        if (cameraPrincipal == null)
            cameraPrincipal = Camera.main;

        proximoIntervalo = ObterNovoIntervalo();
        DesativarVisual();
    }

    void Update()
    {
        if (player == null) return;

        // Espera até ao próximo spawn
        if (!ativo)
        {
            cronometro += Time.deltaTime;
            if (cronometro >= proximoIntervalo)
            {
                AparecerFrenteDaBaleia();
            }
        }

        // Enquanto visível à frente da baleia
        if (ativo && !emFuga)
        {
            Vector3 frente = player.forward * distanciaFrontal;
            transform.position = player.position + frente + offsetLocal;

            tempoAtivo += Time.deltaTime;

            if (tempoAtivo >= tempoVisivel)
            {
                IniciarFugaLateral();
            }
        }

        // Fase de fuga lateral
        if (ativo && emFuga)
        {
            Vector3 novaPosicao = transform.position;

            // Move lateralmente
            novaPosicao += direcaoFuga * velocidadeFugaLateral * Time.deltaTime;

            // Acompanha a baleia no eixo Z
            novaPosicao.z = player.position.z + distanciaFrontal;

            transform.position = novaPosicao;

            Debug.DrawLine(transform.position, transform.position + direcaoFuga, Color.red, 0.1f);

            if (SaiuDoViewport())
            {
                ativo = false;
                emFuga = false;
                DesativarVisual();
                cronometro = 0f;
                proximoIntervalo = ObterNovoIntervalo();
                Debug.Log("Polvo saiu da viewport e será reciclado.");
            }
        }
    }

    void AparecerFrenteDaBaleia()
    {
        ativo = true;
        emFuga = false;
        tempoAtivo = 0f;
        cronometro = 0f;

        transform.position = player.position + player.forward * distanciaFrontal + offsetLocal;
        transform.LookAt(player);

        AtivarVisual();
        Debug.Log("Polvo apareceu à frente da baleia.");

        StartCoroutine(LancarJatosDeTinta());

    }

    void IniciarFugaLateral()
    {
        emFuga = true;

        // Direção de fuga aleatória
        direcaoFuga = Random.value < 0.5f ? Vector3.left : Vector3.right;

        // Rotaciona o polvo para a direção da fuga
        if (direcaoFuga == Vector3.left)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        Debug.Log("Polvo iniciou fuga para o lado: " + (direcaoFuga == Vector3.left ? "ESQUERDA" : "DIREITA"));
    }


    bool SaiuDoViewport()
    {
        Vector3 viewPos = cameraPrincipal.WorldToViewportPoint(transform.position);
        return viewPos.x < -0.1f || viewPos.x > 1.1f;
    }

    float ObterNovoIntervalo()
    {
        return Random.Range(intervaloMinimo, intervaloMaximo);
    }

    void AtivarVisual()
    {
        foreach (var rend in GetComponentsInChildren<Renderer>())
            rend.enabled = true;
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = true;
    }

    void DesativarVisual()
    {
        foreach (var rend in GetComponentsInChildren<Renderer>())
            rend.enabled = false;
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    private IEnumerator LancarJatosDeTinta()
    {
        Debug.Log(">>> A lançar tinta! Número de jatos: " + numeroJatos);

        for (int i = 0; i < numeroJatos; i++)
        {
            if (tintaPrefab != null)
            {
                GameObject tinta = Instantiate(tintaPrefab, transform.position, Quaternion.identity);

                Vector3 direcaoParaPlayer = (player.position - transform.position).normalized;
                Debug.DrawRay(transform.position, direcaoParaPlayer * 3f, Color.red, 2f);

                ParticleSystem ps = tinta.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var main = ps.main;
                    main.startSpeed = 0; // evita conflito com velocidade normal
                    main.simulationSpace = ParticleSystemSimulationSpace.World;

                    var vel = ps.velocityOverLifetime;
                    vel.enabled = true;
                    vel.space = ParticleSystemSimulationSpace.World;

                    // Aplica velocidade real em direção ao jogador
                    vel.x = direcaoParaPlayer.x * 5f;
                    vel.y = direcaoParaPlayer.y * 5f;
                    vel.z = direcaoParaPlayer.z * 5f;
                }
                else
                {
                    Debug.LogWarning("ParticleSystem não encontrado no prefab de tinta.");
                }
            }
            else
            {
                Debug.LogWarning("!!! tintaPrefab não está atribuído.");
            }

            yield return new WaitForSeconds(intervaloEntreJatos);
        }
    }

}
