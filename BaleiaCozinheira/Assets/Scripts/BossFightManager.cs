using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFightManager : MonoBehaviour
{
    public void TerminarBossFight()
    {
        // Vai para a cena GameOver
        SceneManager.LoadScene("GameOver");
    }
}
