using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    public Image escurecedorHUD; // painel preto semi-transparente

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void EscurecerHUD(float duracao)
    {
        StartCoroutine(EfeitoEscurecer(duracao));
    }

    private IEnumerator EfeitoEscurecer(float duracao)
    {
        if (escurecedorHUD == null) yield break;

        // Aplica escurecimento imediato
        escurecedorHUD.color = new Color(0, 0, 0, 0.97f);
        yield return new WaitForSeconds(duracao);

        // Fade-out suave (desvanecer)
        float tempo = 0f;
        float fadeDuration = 1f; // Tempo para desvanecer
        Color corAtual = escurecedorHUD.color;
        Color corDestino = new Color(0, 0, 0, 0f);

        while (tempo < fadeDuration)
        {
            tempo += Time.deltaTime;
            escurecedorHUD.color = Color.Lerp(corAtual, corDestino, tempo / fadeDuration);
            yield return null;
        }

        escurecedorHUD.color = corDestino; // Garante que termina completamente transparente
    }
}
