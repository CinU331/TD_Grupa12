using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject camera;
    private List<GameObject> buildingSpotsObjects;
    public Boolean IsPaused;
    private int build = 1;
    // build = 1 -> building is off | build = 2 -> building is on

    private ShopController shopController;
    private GameResources gameResources;

    public void Start()
    {
        buildingSpotsObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("BuildingSpot"));
        shopController = shopPanel.GetComponentInChildren<ShopController>();

        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
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

        if (Physics.Raycast(ray, out hit, float.MaxValue, layer_mask))
        {
            if (hit.transform.gameObject.tag == "BuildingSpot" && build == 2)
            {
                BuildingSpot buildingSpot = hit.transform.gameObject.GetComponent<BuildingSpot>();
                string chosenTower = shopController.getCurrentlySelectedTower();

                if (Input.GetMouseButtonUp(0) && !buildingSpot.IsOccupied() && !String.IsNullOrEmpty(chosenTower))
                {
                    buildingSpot.CreateTower(chosenTower);
                    buildingSpot.SpawnRocks();

                    gameResources.ChangeCreditsCount(-GetTowerCost(chosenTower));
                }
                else if (Input.GetMouseButtonUp(1) && buildingSpot.IsOccupied())
                {
                    string soldTowerName = buildingSpot.SellTower();
                    buildingSpot.SpawnRocks();

                    if (soldTowerName != null && soldTowerName != "")
                    {
                        gameResources.ChangeCreditsCount(GetTowerCost(soldTowerName));
                    }
                }
                else if (Input.GetMouseButtonUp(2))
                {
                    bool isMockRangeCreated = false;
                    SpawnRockMock(hit, out isMockRangeCreated);

                    if (!isMockRangeCreated && buildingSpot != null)
                    {
                        buildingSpot.SpawnRocksMock(0);
                    }
                }
            }
        }
    }

    private void SpawnRockMock(RaycastHit hit, out bool isMockRangeCreated)
    {
        isMockRangeCreated = false;
        int val = 0;

        switch (shopController.getCurrentlySelectedTower())
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

    public static int GetTowerCost(string towerName)
    {
        switch (towerName)
        {
            case "MagicalTowerItem":
                return 20;
            case "CannonTowerItem":
                return 50;
            case "ArcherTowerItem":
                return 30;
            default:
                Debug.LogError("Tower of given name is not defined!");
                return 1000000;
        }
    }

    public int GetBuildVariable()
    {
        return this.build;
    }

}

