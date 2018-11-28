using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.CharacterController;

public class PauseMenu : MonoBehaviour
{
    public GameObject characterCamera;
    public GameObject tacticalCamera;

    public TacticalCameraMovement tacticalCameraMovement;

    public BuildController buildController;

    public static bool IsPaused = false;
    public GameObject pauseMenuUI;

    private bool charracterCam = false;

    void Start()
    {
        IsPaused = false;
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

        if(charracterCam)
        {
            charracterCam = false;
            characterCamera.GetComponent<Camera>().enabled = true;
            characterCamera.GetComponent<AudioListener>().enabled = true;

            tacticalCamera.GetComponent<Camera>().enabled = false;
            tacticalCamera.GetComponent<AudioListener>().enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            vThirdPersonMotor.lockMovement = false;
        }


        Time.timeScale = 1f;
        if (buildController.IsPaused)
        {
            Time.timeScale = 0f;
        }
        tacticalCameraMovement.isMovementRestricted = false;
        IsPaused = false;

        

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        if (characterCamera.GetComponent<Camera>().enabled == true)
        {
            charracterCam = true;
            tacticalCamera.GetComponent<Camera>().enabled = true;
            tacticalCamera.GetComponent<AudioListener>().enabled = true;

            characterCamera.GetComponent<Camera>().enabled = false;
            characterCamera.GetComponent<AudioListener>().enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            vThirdPersonMotor.lockMovement = true;
        }

        Time.timeScale = 0f;
        tacticalCameraMovement.isMovementRestricted = true;
        IsPaused = true;
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
