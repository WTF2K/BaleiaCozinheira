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
        SceneManager.LoadScene("Game");
    }
    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void GoToTutorial1()
    {
        SceneManager.LoadScene("Tutorial_1");
    }
    public void GoToTutorial2()
    {
        SceneManager.LoadScene("Tutorial_2");
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
