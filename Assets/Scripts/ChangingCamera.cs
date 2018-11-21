using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingCamera : MonoBehaviour {

    public GameObject camera1;
    public GameObject camera2;

    public BuildController buildController;

    int startCamera = 1;
    public bool zmiana = false;

    public bool canChangeCamera = true;


    // Use this for initialization
    void Start()
    {
        buildController.Start();
        buildController.StartBuild();
        camera1.GetComponent<Camera>().enabled = true;
        camera1.GetComponent<AudioListener>().enabled = true;

        camera2.GetComponent<Camera>().enabled = false;
        camera2.GetComponent<AudioListener>().enabled = false;
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

        camera1.GetComponent<Camera>().enabled = true;
        camera1.GetComponent<AudioListener>().enabled = true;

        camera2.GetComponent<Camera>().enabled = false;
        camera2.GetComponent<AudioListener>().enabled = false;
        this.zmiana = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        buildController.StartBuild();
    }

    public void SetActiveThirdCam()
    {
        startCamera = 2;

        camera1.GetComponent<Camera>().enabled = false;
        camera1.GetComponent<AudioListener>().enabled = false;

        camera2.GetComponent<Camera>().enabled = true;
        camera2.GetComponent<AudioListener>().enabled = true;
        this.zmiana = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        buildController.StopBuild();
    }
}