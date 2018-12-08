using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public static int AvailableSpikeTraps = 3;
    public static int AvailableSplashTraps = 3;

    public TextMeshProUGUI ArcherTowerCostText;
    public TextMeshProUGUI MagicalTowerCostText;
    public TextMeshProUGUI CannonTowerCostText;

    private List<Transform> shopButtons;

    private Transform selectedTower;
    private GameResources gameResources;


    // Use this for initialization
    private void Start()
    {
        selectedTower = null;
        shopButtons = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Shop"))
            {
                shopButtons.Add(child);
                child.GetComponent<Button>().onClick.AddListener(() => ShopButtonClicked(child));
            }
        }

        ArcherTowerCostText.text = TowerCost.ArcherTowerCost.ToString();
        MagicalTowerCostText.text = TowerCost.MagicalTowerCost.ToString();
        CannonTowerCostText.text = TowerCost.CannonTowerCost.ToString();

        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        gameResources.CreditsChanged += UpdateTowerAvailability;

        UpdateTowerAvailability();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void UpdateTowerAvailability()
    {
        SetAppropriateColors();
        CheckTowerCostAndSetIfValid(selectedTower);
    }

    public string getCurrentlySelectedTower()
    {
        if (selectedTower != null)
        {
            return selectedTower.name;
        }

        return "";
    }

    public void ShopButtonClicked(Transform button)
    {
        SetAppropriateColors();

        if (button != null && button != selectedTower)
        {
            CheckTowerCostAndSetIfValid(button);
        }
        else
        {
            selectedTower = null;
        }
    }

    private void CheckTowerCostAndSetIfValid(Transform button)
    {
        if (button)
        {
            ShopButtonControler buttonControler = button.GetComponent<ShopButtonControler>();

            if (buttonControler != null && buttonControler.CurrentColor == ShopButtonControler.COLOR_AVAILABLE)
            {
                buttonControler.SetForegroundColor(ShopButtonControler.COLOR_SELECTED);
                selectedTower = button;
            }
            else
            {
                selectedTower = null;
            }
        }
    }

    private void SetAppropriateColors()
    {
        int currentCredits = gameResources.Credits;
        foreach (Transform shopButton in shopButtons)
        {
            ShopButtonControler otherShopButton = shopButton.GetComponent<ShopButtonControler>();
            if (otherShopButton != null)
            {
                if (currentCredits >= BuildController.GetTowerCost(shopButton.name))
                {
                    otherShopButton.SetForegroundColor(ShopButtonControler.COLOR_AVAILABLE);
                }
                else
                {
                    otherShopButton.SetForegroundColor(ShopButtonControler.COLOR_DISABLED);
                }
            }
        }
    }
}
