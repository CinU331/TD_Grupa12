using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildController : MonoBehaviour
    {
        public GameObject shopPanel;
        public GameObject camera;
        
        public Boolean IsPaused;
        int build = 1;

        

        // Use this for initialization
        void Start()
        {
            shopPanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            Boolean change = false;
            if (camera.GetComponent<Camera>().enabled == true && Input.GetKeyDown("b") && (build == 1) && !change)
            {
                build = 2;
                StartBuild();
                change = true;
            }
            if (camera.GetComponent<Camera>().enabled == true && Input.GetKeyDown("b") && (build == 2) && !change)
            {
                build = 1;
                StopBuild();
                change = true;
            }
            if (camera.GetComponent<Camera>().enabled == false && !change)
            {
                build = 1;
                StopBuild();
            }
        }       

        public void StartBuild()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            IsPaused = true;

            GameObject buildingSpots = GameObject.Find("BuildingSpots");
            shopPanel.SetActive(true);

            foreach (Transform transform in buildingSpots.transform)
            {
                GameObject lightGameObject = new GameObject();
                lightGameObject.tag = "Information";
                Light lightComp = lightGameObject.AddComponent<Light>();
                lightComp.color = Color.green;
                lightComp.range = 5;
                lightComp.intensity = 100;
                lightGameObject.transform.position = transform.position;
            }
        }

        public void StopBuild()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            IsPaused = false;

            shopPanel.SetActive(false);

            GameObject [] lights = GameObject.FindGameObjectsWithTag("Information");
            for(int i = 0; i < lights.Length; i++)
            {
                GameObject.Destroy(lights[i]);
            }
        }
    }
}
