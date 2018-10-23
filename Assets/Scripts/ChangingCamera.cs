using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingCamera : MonoBehaviour {

    public Camera camera1; //kamery
    public Camera camera2;

    int startCamera = 1;

    // Use this for initialization
    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        bool zmiana = false;
        if (Input.GetKeyDown("c") && (startCamera == 1) && !zmiana)
        {
            startCamera = 2;

            camera1.enabled = false;
            camera2.enabled = true;
            zmiana = true;
        }
        if (Input.GetKeyDown("c") && (startCamera == 2) && !zmiana)
        {
            startCamera = 1;

            camera1.enabled = true;
            camera2.enabled = false;
            zmiana = true;
        }
    }
}