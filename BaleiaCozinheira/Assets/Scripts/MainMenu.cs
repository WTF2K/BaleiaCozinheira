using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameTrials");
    }
    public void GoToIntroScene()
    {
        SceneManager.LoadScene("IntroScene");
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
        SceneManager.LoadScene("Credits");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
