using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject camera;
    private List<GameObject> buildingSpotsObjects;
    public Boolean IsPaused;
    private int build = 1;
    // build = 1 -> building is off | build = 2 -> building is on

    public GameObject magicalTower;
    public GameObject cannonTower;
    public GameObject archerTower;

    public GameObject magicalTowerUpgradeUI;
    public GameObject cannonTowerUpgradeUI;
    public GameObject archerTowerUpgradeUI;

    public GameObject currentUpgradeUI;
    public AbstractTower currentUpgradingTurret;

    private ShopController shopController;
    private GameResources gameResources;
    private GameObject shop;

    public Material ConstructionHighlightMaterial;
    private GameObject constructionHighlight;
    private bool isConstructionHighlightActive;
    private bool isHighlightSnapped;
    private string currentConstructionTower;


    public void Start()
    {
        shop = GameObject.Find("Shop");
        buildingSpotsObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("BuildingSpot"));
        shopController = shopPanel.GetComponentInChildren<ShopController>();

        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();

        SetupTowerConstructionHighlight();
    }


    public void StartBuild()
    {

        build = 2;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        IsPaused = true;

        shop.SetActive(true);

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

        shop.SetActive(false);
        DisableConstructionHighlight();

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
        if (currentUpgradeUI != null)
        {
            UpdateInfoAboutUpgrade(currentUpgradeUI, currentUpgradingTurret);
        }

        if (EventSystem.current.IsPointerOverGameObject()) // if blocked by ui
        {
            DisableConstructionHighlight();
            return;
        }

        string chosenTower = shopController.getCurrentlySelectedTower();
        if (!String.IsNullOrEmpty(chosenTower))
        {
            EnableConstructionHighlight(chosenTower);
        }
        else
        {
            DisableConstructionHighlight();
        }

        int layerMask = LayerMask.GetMask("BuildLayer");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isSnapped = false;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
        {
            if (hit.transform.gameObject.CompareTag("BuildingSpot") && build == 2)
            {
                BuildingSpot buildingSpot = hit.transform.gameObject.GetComponent<BuildingSpot>();

                if (!buildingSpot.IsOccupied())
                {
                    isSnapped = true;
                    UpdateConstructionHighlightPositionSnapped(hit.transform.gameObject.transform.position);
                    GameObject tmpTower = GetTowerInstance(chosenTower);

                    if (chosenTower != "")
                    {
                        buildingSpot.SpawnRocksMock(tmpTower);
                    }

                    Destroy(tmpTower);
                }

                if (Input.GetMouseButtonUp(0) && !buildingSpot.IsOccupied() && !String.IsNullOrEmpty(chosenTower))
                {
                    GameObject newTower = GetTowerInstance(chosenTower);
                    buildingSpot.CreateTower(newTower);
                    newTower.GetComponent<AbstractTower>().BuildingSpot = buildingSpot;
                    buildingSpot.SpawnRocks();

                    gameResources.ChangeCreditsCount(-GetTowerCost(chosenTower));
                }
                else if (Input.GetMouseButtonUp(1) && buildingSpot.IsOccupied())
                {
                    string soldTowerName = buildingSpot.SellTower();
                    buildingSpot.SpawnRocks();

                    if (!string.IsNullOrEmpty(soldTowerName))
                    {
                        gameResources.ChangeCreditsCount(GetTowerCost(soldTowerName) + buildingSpot.GetUpgradesCost());
                    }
                }
            }

            foreach (GameObject bs in buildingSpotsObjects)
            {
                if (bs != hit.transform.gameObject)
                {
                    bs.SendMessage("DestroyMockRocks");
                }
            }


        }
        else
        {
            foreach (GameObject bs in buildingSpotsObjects)
            {
                bs.SendMessage("DestroyMockRocks");
            }

            if (Input.GetMouseButtonUp(1))
            {
                DisableConstructionHighlight();
                shopController.DeselectTower();
            }
        }

        int towersLayerMask = LayerMask.GetMask("Towers");
        if (Physics.Raycast(ray, out hit, float.MaxValue, towersLayerMask))
        {
            if (hit.transform.gameObject.CompareTag("Tower"))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Destroy(currentUpgradeUI);
                    currentUpgradingTurret = hit.transform.gameObject.GetComponent<AbstractTower>();

                    currentUpgradeUI = DisplayUpgradeUi(currentUpgradingTurret);
                    Transform upgradeButton = currentUpgradeUI.transform.FindDeepChild("UpgradeButton");
                    upgradeButton.gameObject.GetComponent<Button>().onClick.AddListener(Upgrade);
                    Transform sellButton = currentUpgradeUI.transform.FindDeepChild("SellButton");
                    sellButton.gameObject.GetComponent<Button>().onClick.AddListener(() => SellTower(currentUpgradingTurret.BuildingSpot));
                }
            }
        }

        if (!isSnapped)
        {
            UpdateConstructionHighlightPosition();
        }
    }

    private void UpdateInfoAboutUpgrade(GameObject upgradeUi, AbstractTower selectedTower)
    {
        string towerIdentificator = selectedTower.TowerIdentificator;
        Transform upgradeButton = upgradeUi.transform.FindDeepChild("UpgradeButton");
        if (!currentUpgradingTurret.IsUpgradeAvailable)
        {
            upgradeButton.gameObject.SetActive(false);
        }


        switch (towerIdentificator)
        {
            case "MagicalTowerItem":

                MageUpgradeUI mageUi = upgradeUi.GetComponentInChildren<MageUpgradeUI>();

                mageUi.DamageNow = ((MageTower)selectedTower).iDamage;
                mageUi.DamageAfter = (int)(((MageTower)selectedTower).iDamage * 1.5f);

                mageUi.MaxTargetsNow = ((MageTower)selectedTower).iMaxTargets;
                mageUi.MaxTargetsAfter = ((MageTower)selectedTower).iMaxTargets + 1;

                mageUi.RangeNow = ((MageTower)selectedTower).iRange;
                mageUi.RangeAfter = ((MageTower)selectedTower).iRange + 2f;

                mageUi.SlowDownFactorNow = ((MageTower)selectedTower).iSlowDownFactor;
                mageUi.SlowDownFactorAfter = ((MageTower)selectedTower).iSlowDownFactor - 0.1f;
                mageUi.SetValues();
                break;
            case "CannonTowerItem":
                CanonUpgradeUI canonUi = upgradeUi.GetComponentInChildren<CanonUpgradeUI>();

                canonUi.DamageNow = ((CanonTower)selectedTower).iDamage;
                canonUi.DamageAfter = (int)(((CanonTower)selectedTower).iDamage * 1.3f);

                canonUi.CooldownNow = ((CanonTower)selectedTower).iCooldown;
                canonUi.CooldownAfter = ((CanonTower)selectedTower).iCooldown * 0.9f;

                canonUi.RangeNow = ((CanonTower)selectedTower).iRange;
                canonUi.RangeAfter = ((CanonTower)selectedTower).iRange + 3f;

                canonUi.SplashRangeNow = ((CanonTower)selectedTower).iSplashRange;
                canonUi.SplashRangeAfter = ((CanonTower)selectedTower).iSplashRange * 1.3f;
                canonUi.SetValues();
                break;
            case "ArcherTowerItem":
                ArcherUpgradeUI archerUi = upgradeUi.GetComponentInChildren<ArcherUpgradeUI>();
                archerUi.DamageNow = ((ArcherTower)selectedTower).iDamage;
                archerUi.DamageAfter = (int)(((ArcherTower)selectedTower).iDamage * 1.5f);

                archerUi.MaxTargetsNow = ((ArcherTower)selectedTower).iMaxTargets;
                archerUi.MaxTargetsAfter = ((ArcherTower)selectedTower).iMaxTargets + 1;

                archerUi.RangeNow = ((ArcherTower)selectedTower).iRange;
                archerUi.RangeAfter = ((ArcherTower)selectedTower).iRange + 2f;

                archerUi.SlowDownFactorNow = ((ArcherTower)selectedTower).iSlowDownRatio;
                archerUi.SlowDownFactorAfter = ((ArcherTower)selectedTower).iSlowDownRatio - 0.1f;

                archerUi.CooldownNow = ((ArcherTower)selectedTower).iCooldown;
                archerUi.CooldownNow = ((ArcherTower)selectedTower).iCooldown - 0.5f;
                archerUi.SetValues();
                break;
        }
    }

    public void SellTower(BuildingSpot buildingSpot)
    {
        string soldTowerName = buildingSpot.SellTower();
        buildingSpot.SpawnRocks();

        if (!string.IsNullOrEmpty(soldTowerName))
        {
            gameResources.ChangeCreditsCount(GetTowerCost(soldTowerName) + buildingSpot.GetUpgradesCost());
        }
        Destroy(currentUpgradeUI);
    }
    public void Upgrade()
    {
        currentUpgradingTurret.SendMessage("UpgradeTower");
    }

    public static int GetTowerCost(string towerName)
    {
        switch (towerName)
        {
            case "MagicalTowerItem":
                return TowerCost.MagicalTowerCost;
            case "CannonTowerItem":
                return TowerCost.CannonTowerCost;
            case "ArcherTowerItem":
                return TowerCost.ArcherTowerCost;
            default:
                Debug.LogError("Tower of given name is not defined!");
                return 1000000;
        }
    }

    private void SetupTowerConstructionHighlight()
    {
        constructionHighlight = new GameObject();
        constructionHighlight.SetActive(false);

        constructionHighlight.transform.parent = GameObject.Find("Terrain").transform;
    }

    private void DisableConstructionHighlight()
    {
        foreach (GameObject bs in buildingSpotsObjects)
        {
            bs.SendMessage("DestroyMockRocks");
        }

        foreach (Transform child in constructionHighlight.transform)
        {
            Destroy(child.gameObject);
        }

        constructionHighlight.SetActive(false);
        isConstructionHighlightActive = false;
        isHighlightSnapped = false;
        currentConstructionTower = "";
    }

    private void EnableConstructionHighlight(string chosenTower)
    {
        if (!isConstructionHighlightActive || chosenTower != null && chosenTower != currentConstructionTower)
        {
            GameObject towerInstance = GetTowerInstance(chosenTower);
            foreach (var childRenderer in towerInstance.GetComponentsInChildren<Renderer>())
            {
                childRenderer.material = ConstructionHighlightMaterial;
            }

            towerInstance.transform.parent = constructionHighlight.transform;
            towerInstance.transform.localPosition = Vector3.zero;

            currentConstructionTower = chosenTower;
            constructionHighlight.SetActive(true);
            isConstructionHighlightActive = true;
        }
    }

    private void UpdateConstructionHighlightPosition()
    {
        if (constructionHighlight != null)
        {
            if (isHighlightSnapped)
            {
                isHighlightSnapped = false;

                Color snappedColor = new Color(1f, 0, 0, 100 / 255f);
                foreach (var childRenderer in constructionHighlight.transform.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = snappedColor;
                }
            }

            int layerMask = LayerMask.GetMask("Terrain");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
            {
                constructionHighlight.transform.position = hitInfo.point;
            }
        }
    }

    private void UpdateConstructionHighlightPositionSnapped(Vector3 transformPosition)
    {
        if (!isHighlightSnapped)
        {
            isHighlightSnapped = true;

            Color snappedColor = new Color(0, 0, 1f, 100 / 255f);
            foreach (var childRenderer in constructionHighlight.transform.GetComponentsInChildren<Renderer>())
            {
                childRenderer.material.color = snappedColor;
            }
        }

        constructionHighlight.transform.position = transformPosition;
    }

    public GameObject DisplayUpgradeUi(AbstractTower tower)
    {
        string towerIdentificator = tower.TowerIdentificator;
        switch (towerIdentificator)
        {
            case "MagicalTowerItem":
                return Instantiate(magicalTowerUpgradeUI);
            case "CannonTowerItem":
                return Instantiate(cannonTowerUpgradeUI);
            case "ArcherTowerItem":
                return Instantiate(archerTowerUpgradeUI);
            default:
                return null;
        }
    }
    public GameObject GetTowerInstance(string towerIdentificator)
    {
        switch (towerIdentificator)
        {
            case "MagicalTowerItem":
                return Instantiate(magicalTower);
            case "CannonTowerItem":
                return Instantiate(cannonTower);
            case "ArcherTowerItem":
                return Instantiate(archerTower);
            default:
                return null;
        }
    }
}

