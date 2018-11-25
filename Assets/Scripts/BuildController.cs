using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject camera;
    private List<GameObject> buildingSpotsObjects;
    private GameObject previousBuildingSpot;
    public Boolean IsPaused;
    private int build = 1;
    // build = 1 -> building is off | build = 2 -> building is on

        
    public void Start()
    {
        buildingSpotsObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("BuildingSpot"));
    }


    public void StartBuild()
    {
        build = 2;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        IsPaused = true;

        shopPanel.SetActive(true);

        foreach (GameObject buildingSpot in buildingSpotsObjects)
        {
            buildingSpot.GetComponent<Collider>().enabled = true;
        }

        foreach (GameObject buildingSpot in buildingSpotsObjects)
        {
            buildingSpot.GetComponent<BuildingSpot>().SendMessage("SetNotOccupiedVisible", true);
        }

    }

    public void StopBuild()
    {
        build = 1;
        Time.timeScale = 1f;
        IsPaused = false;

        shopPanel.SetActive(false);

        foreach (GameObject buildingSpot in buildingSpotsObjects)
        {
            buildingSpot.GetComponent<Collider>().enabled = false;
        }

        foreach (GameObject buildingSpot in buildingSpotsObjects)
        {
            buildingSpot.GetComponent<BuildingSpot>().SendMessage("SetNotOccupiedVisible", false);
        }
    }

    public void BuildingTowers()
    {
        if(EventSystem.current.IsPointerOverGameObject()) // if blocked by ui
        {
            return;
        }

        int layer_mask = LayerMask.GetMask("BuildLayer");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isMockRangeCreated = false;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layer_mask))
        {
            if (hit.transform.gameObject.tag == "BuildingSpot" && build == 2)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    hit.transform.gameObject.SendMessage("CreateTower", ShopButtonControler.towerButtonClicked);
                    hit.transform.gameObject.SendMessage("SpawnRocks");
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    hit.transform.gameObject.SendMessage("SellTower");
                    hit.transform.gameObject.SendMessage("SpawnRocks");
                }
                else if (Input.GetMouseButtonUp(2))
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

    public int GetBuildVariable()
    {
        return this.build;
    }

}

