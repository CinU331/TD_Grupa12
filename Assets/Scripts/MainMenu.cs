using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().name.ToString() == "GameOver")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("Terrain");
        }

        GameResources gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        gameResources.Credits = 10;
        gameResources.Resources = 5;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
