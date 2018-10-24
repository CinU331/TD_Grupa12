using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingCamera : MonoBehaviour {

    public GameObject camera1;
    public GameObject camera2;

    int startCamera = 1;

    // Use this for initialization
    void Start()
    {
        camera1.GetComponent<Camera>().enabled = true;
        camera1.GetComponent<AudioListener>().enabled = true;

        camera2.GetComponent<Camera>().enabled = false;
        camera2.GetComponent<AudioListener>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        bool zmiana = false;
        if (Input.GetKeyDown("c") && (startCamera == 1) && !zmiana)
        {
            startCamera = 2;

            camera1.GetComponent<Camera>().enabled = false;
            camera1.GetComponent<AudioListener>().enabled = false;

            camera2.GetComponent<Camera>().enabled = true;
            camera2.GetComponent<AudioListener>().enabled = true;
            zmiana = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown("c") && (startCamera == 2) && !zmiana)
        {
            startCamera = 1;

            camera1.GetComponent<Camera>().enabled = true;
            camera1.GetComponent<AudioListener>().enabled = true;

            camera2.GetComponent<Camera>().enabled = false;
            camera2.GetComponent<AudioListener>().enabled = false;
            zmiana = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}