using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame(int characterIndex)
    {
        SceneManager.LoadScene("Game");
    }
    public void GoToScoreBoard(int characterIndex)
    {
        SceneManager.LoadScene("Scoreboard");
    }
    public void GoToOptions(int characterIndex)
    {
        SceneManager.LoadScene("Game");
    }
    public void GoToCredits(int characterIndex)
    {
        SceneManager.LoadScene("Credits");
    }
    public void ExitGame(int characterIndex)
    {
        Application.Quit();
    }
}
