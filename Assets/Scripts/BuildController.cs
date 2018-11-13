using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildController : MonoBehaviour
    {
        public GameObject shopPanel;
        public GameObject camera;
        List<GameObject> buildingSpotsObjects;
        List<BuildingSpot> buildingSpots;
        public Boolean IsPaused;
        private int build = 1;



        // Use this for initialization
        private void Start()
        {
            buildingSpotsObjects = new List<GameObject>();
            buildingSpots = new List<BuildingSpot>();

            GameObject spotsSource = GameObject.Find("BuildingSpots");
            int i = 0;


            foreach (Transform transform in spotsSource.transform)
            {
                buildingSpotsObjects.Add(transform.gameObject);
                buildingSpots.Add(buildingSpotsObjects[i].GetComponent<BuildingSpot>());
                buildingSpotsObjects[i].tag = "BuildingSpot";

                Light lightComp = buildingSpotsObjects[i].AddComponent<Light>();
                lightComp.color = Color.green;
                lightComp.range = 2.5f;
                lightComp.intensity = 50;
                lightComp.enabled = false;
                buildingSpotsObjects[i].transform.position += new Vector3(0, 0.1f, 0);
                i++;
            }
            shopPanel.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "BuildingSpot")
                {
                    if (Input.GetMouseButton(0))
                    {
                        hit.transform.gameObject.SendMessage("CreateTower", "MagicalTower");
                    }
                    else if(Input.GetMouseButton(1))
                    {
                        hit.transform.gameObject.SendMessage("SellTower");
                    }
                }
            }

        }

        public void StartBuild()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            IsPaused = true;

            shopPanel.SetActive(true);

            foreach (GameObject buildingSpot in buildingSpotsObjects)
            {

                buildingSpot.GetComponent<Collider>().enabled = true;
                buildingSpot.GetComponent<Light>().enabled = true;

            }
        }

        public void StopBuild()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            IsPaused = false;

            shopPanel.SetActive(false);

            foreach(GameObject buildingSpot in buildingSpotsObjects)
            {
                buildingSpot.GetComponent<Collider>().enabled = false;
                buildingSpot.GetComponent<Light>().enabled = false;
            }
        }
    }
}
