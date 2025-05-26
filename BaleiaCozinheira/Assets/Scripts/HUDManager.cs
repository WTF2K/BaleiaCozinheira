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
        StopAllCoroutines();
        StartCoroutine(FazerEscurecer(duracao));
    }

    System.Collections.IEnumerator FazerEscurecer(float tempo)
    {
        escurecedorHUD.enabled = true;
        escurecedorHUD.color = new Color(0, 0, 0, 0.8f); // escuro

        yield return new WaitForSeconds(tempo);

        escurecedorHUD.enabled = false;
    }
}
