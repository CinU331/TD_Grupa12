using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().name.ToString() == "GameOver")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            Enemies.credits = 10;
            Enemies.resources = 5;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Enemies.credits = 10;
            Enemies.resources = 5;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
