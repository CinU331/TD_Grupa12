using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject characterCamera;
    public GameObject tacticalCamera;

    public TacticalCameraMovement tacticalCameraMovement;

    public BuildController buildController;

    public static bool IsPaused = false;
    public GameObject pauseMenuUI;

    private string cam = "";

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        if(cam == "characterCamera")
        {
            cam = "";
            characterCamera.GetComponent<Camera>().enabled = true;
            characterCamera.GetComponent<AudioListener>().enabled = true;

            tacticalCamera.GetComponent<Camera>().enabled = false;
            tacticalCamera.GetComponent<AudioListener>().enabled = false;            
        }
        

        Time.timeScale = 1f;
        if (buildController.IsPaused)
        {
            Time.timeScale = 0f;
        }
        tacticalCameraMovement.isMovementRestricted = false;
        IsPaused = false;

        //Cursor.lockState = CursorLockMode.Locked;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        if (characterCamera.GetComponent<Camera>().enabled == true)
        {
            cam = "characterCamera";
            tacticalCamera.GetComponent<Camera>().enabled = true;
            tacticalCamera.GetComponent<AudioListener>().enabled = true;

            characterCamera.GetComponent<Camera>().enabled = false;
            characterCamera.GetComponent<AudioListener>().enabled = false;
        }

        Time.timeScale = 0f;
        tacticalCameraMovement.isMovementRestricted = true;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
