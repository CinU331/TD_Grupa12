using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildController : MonoBehaviour
    {
        public GameObject shopPanel;
        public GameObject camera;
        private List<GameObject> buildingSpotsObjects;
        private GameObject previousBuildingSpot;
        public Boolean IsPaused;
        private int build = 1;
        //public ShopButtonControler shopButtonControler;



        // Use this for initialization
        private void Start()
        {
            buildingSpotsObjects = new List<GameObject>();

            GameObject spotsSource = GameObject.Find("BuildingSpots");
            int i = 0;


            foreach (Transform transform in spotsSource.transform)
            {
                buildingSpotsObjects.Add(transform.gameObject);
                buildingSpotsObjects[i].tag = "BuildingSpot";
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


            foreach (GameObject spot in buildingSpotsObjects)
            {
                if (build == 1)
                {
                    spot.GetComponent<BuildingSpot>().SetNotOccupiedVisible(false);
                }
                else
                {
                    spot.GetComponent<BuildingSpot>().SetNotOccupiedVisible(true);
                }
            }


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool isMockRangeCreated = false;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "BuildingSpot" && build == 2)
                {
                    if (Input.GetMouseButton(0))
                    {
                        hit.transform.gameObject.SendMessage("CreateTower", ShopButtonControler.towerButtonClicked);
                        hit.transform.gameObject.SendMessage("SpawnRocks");
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        hit.transform.gameObject.SendMessage("SellTower");
                        hit.transform.gameObject.SendMessage("SpawnRocks");
                    }
                    else if (Input.GetMouseButton(2))
                    {
                        int val = 0;
                        switch (ShopButtonControler.towerButtonClicked)
                        {
                            case "MagicalTowerItem":
                                {
                                    val = 1;
                                    break;
                                }
                            case "CannonTowerItem":
                                {
                                    val = 2;
                                    break;
                                }
                            case "ArcherTowerItem":
                                {
                                    val = 3;
                                    break;
                                }
                        }
                        hit.transform.gameObject.SendMessage("SpawnRocksMock", val);
                        if (val != 0)
                        {
                            isMockRangeCreated = true;
                        }
                    }
                    previousBuildingSpot = hit.transform.gameObject;
                }
                if (!isMockRangeCreated)
                {
                    int zeroValue = 0;
                    if (previousBuildingSpot != null)
                    {
                        previousBuildingSpot.SendMessage("SpawnRocksMock", zeroValue);
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
            }

        }

        public void StopBuild()
        {
            Time.timeScale = 1f;
            //Cursor.lockState = CursorLockMode.Locked;
            IsPaused = false;

            shopPanel.SetActive(false);

            foreach (GameObject buildingSpot in buildingSpotsObjects)
            {
                buildingSpot.GetComponent<Collider>().enabled = false;
            }
        }

    }
}
