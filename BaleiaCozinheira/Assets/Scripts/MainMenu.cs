using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void GoToScoreBoard()
    {
        SceneManager.LoadScene("Scoreboard");
    }
    public void GoToOptions()
    {
        SceneManager.LoadScene("Options");
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("GameCredits");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
