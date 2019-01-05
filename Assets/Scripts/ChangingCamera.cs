using System.Collections;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;

public class ChangingCamera : MonoBehaviour
{

    public GameObject TacticalCamera;
    public GameObject ThirdPersonCamera;
    public BuildController buildController;
    public GameObject playerHUD;

    int startCamera = 1;
    public bool zmiana = false;

    public bool canChangeCamera = true;

    // Use this for initialization
    void Start()
    {
        buildController.Start();
        SetActiveTacticalCam(playMusic: true);
    }

    // Update is called once per frame
    void Update()
    {
        zmiana = false;
        if (Input.GetKeyDown("c") && (startCamera == 1) && !zmiana && canChangeCamera)
        {
            SetActiveThirdCam(playMusic: false);
        }
        if (Input.GetKeyDown("c") && (startCamera == 2) && !zmiana && canChangeCamera)
        {
            SetActiveTacticalCam(playMusic: false);
        }

        if (startCamera == 1)
        {
            buildController.BuildingTowers();
        }
    }

    public void SetActiveTacticalCam(bool playMusic)
    {
        startCamera = 1;

        TacticalCamera.GetComponent<Camera>().enabled = true;

        ThirdPersonCamera.GetComponent<Camera>().enabled = false;
        if (playMusic)
        {
            ThirdPersonCamera.GetComponent<AudioSource>().Stop();
            ThirdPersonCamera.GetComponent<AudioListener>().enabled = false;
            TacticalCamera.GetComponent<AudioListener>().enabled = true;
            TacticalCamera.GetComponent<AudioSource>().Play();
        }

        this.zmiana = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerHUD.SetActive(false);

        buildController.StartBuild();
        vThirdPersonMotor.lockMovement = true;
    }

    public void SetActiveThirdCam(bool playMusic)
    {
        ResetUpgradeUI();
        startCamera = 2;

        TacticalCamera.GetComponent<Camera>().enabled = false;
        ThirdPersonCamera.GetComponent<Camera>().enabled = true;
        if (playMusic)
        {
            TacticalCamera.GetComponent<AudioSource>().Stop();
            TacticalCamera.GetComponent<AudioListener>().enabled = false;

            ThirdPersonCamera.GetComponent<AudioListener>().enabled = true;
            ThirdPersonCamera.GetComponents<AudioSource>()[1].Play();
            StartCoroutine(FadeIn(ThirdPersonCamera.GetComponents<AudioSource>()[0], 0.001f, 0f, 0.2f));
        }

        this.zmiana = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerHUD.SetActive(true);

        buildController.StopBuild();
        vThirdPersonMotor.lockMovement = false;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float speed, float startVolume, float maxVolume)
    {
        audioSource.volume = startVolume;
        audioSource.Play();
        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += speed;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void ResetUpgradeUI()
    {
        Destroy(GetComponent<BuildController>().currentUpgradeUI);
    }
}