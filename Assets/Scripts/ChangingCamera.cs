using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingCamera : MonoBehaviour {

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
        SetActiveTacticalCam();
    }

    // Update is called once per frame
    void Update ()
    {
        zmiana = false;
        if (Input.GetKeyDown("c") && (startCamera == 1) && !zmiana && canChangeCamera)
        {
            SetActiveThirdCam();
        }
        if (Input.GetKeyDown("c") && (startCamera == 2) && !zmiana && canChangeCamera)
        {
            SetActiveTacticalCam();
        }
      
        if(startCamera == 1)
        {
            buildController.BuildingTowers();
        }        
    }

    public void SetActiveTacticalCam()
    {
        startCamera = 1;

        TacticalCamera.GetComponent<Camera>().enabled = true;
        TacticalCamera.GetComponent<AudioListener>().enabled = true;

        ThirdPersonCamera.GetComponent<Camera>().enabled = false;
        ThirdPersonCamera.GetComponent<AudioListener>().enabled = false;
        this.zmiana = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerHUD.SetActive(false);

        buildController.StartBuild();
    }

    public void SetActiveThirdCam()
    {
        startCamera = 2;

        TacticalCamera.GetComponent<Camera>().enabled = false;
        TacticalCamera.GetComponent<AudioListener>().enabled = false;

        ThirdPersonCamera.GetComponent<Camera>().enabled = true;
        ThirdPersonCamera.GetComponent<AudioListener>().enabled = true;
        this.zmiana = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerHUD.SetActive(true);

        buildController.StopBuild();
    }
}