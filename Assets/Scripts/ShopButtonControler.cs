using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButtonControler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Color COLOR_SELECTED = new Color(95/255f, 95/255f, 255/255f, 20/255f);
    public static Color COLOR_AVAILABLE = new Color(0/255f, 185/255f, 35/255f, 10/255f);
    public static Color COLOR_DISABLED = new Color(255/255f, 30/255f, 0/255f, 10/255f);

    public Color CurrentColor { get; private set; }
    public GameObject RepresentedTower;
    private AbstractTower tower;

    public GameObject tooltipPrefab;
    private GameObject tooltipInstance;
    private RectTransform backgroundTransform;
    private bool isTooltipVisible;

    void Start()
    {
        tooltipInstance = Instantiate(tooltipPrefab);
        tooltipInstance.SetActive(false);
        isTooltipVisible = false;

        Transform tooltipBackground = tooltipInstance.transform.Find("TooltipBackground");
        backgroundTransform = tooltipBackground.GetComponent<RectTransform>();

        tower = RepresentedTower.GetComponent<AbstractTower>();

        tooltipBackground.Find("Name").GetComponent<TextMeshProUGUI>().text = tower.name;
        tooltipBackground.Find("Damage").GetComponent<TextMeshProUGUI>().text = "Damage: " + tower.iDamage;
        tooltipBackground.Find("Range").GetComponent<TextMeshProUGUI>().text = "Range: " + tower.iRange;
        tooltipBackground.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: " + BuildController.GetTowerCost(tower.TowerIdentificator);
    }

    private void Update()
    {
        if (isTooltipVisible)
        {
            backgroundTransform.position = new Vector3(backgroundTransform.rect.size.x / 4, backgroundTransform.rect.size.y / 4) +
                                           Input.mousePosition +  new Vector3(-90, 20);
        }
    }

    public void SetForegroundColor(Color color) 
    {
        Image foregroundImage = transform.Find("SelectionImage").GetComponent<Image>();
        if (foregroundImage != null) 
        {
            foregroundImage.color = color;
            CurrentColor = color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipInstance.SetActive(true);
        isTooltipVisible = true;
        backgroundTransform.position = Input.mousePosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipInstance.SetActive(false);
        isTooltipVisible = false;
    }
}
