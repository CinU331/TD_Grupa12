using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopTrapController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Color COLOR_AVAILABLE = new Color(0/255f, 185/255f, 37/255f, 20/255f);
    public static Color COLOR_DISABLED = new Color(255/255f, 30/255f, 0/255f, 10/255f);

    public Color CurrentColor { get; private set; }
    public TextMeshProUGUI CostText;
    public GameObject RepresentedTrap;
    private AbstractTrap trap;

    public GameObject tooltipPrefab;
    private GameResources gameResources;
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

        trap = RepresentedTrap.GetComponent<AbstractTrap>();

        tooltipBackground.Find("Name").GetComponent<TextMeshProUGUI>().text = trap.Name;
        tooltipBackground.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: " + trap.Cost;
        
        CostText.text = trap.Cost.ToString();
        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        gameResources.TrapsChanged += UpdateAvailability;
        gameResources.ResourcesChanged += SetAppropriateColors;

        UpdateAvailability();
    }

    private void Update()
    {
        if (isTooltipVisible)
        {
            backgroundTransform.position = new Vector3(backgroundTransform.rect.size.x / 4, backgroundTransform.rect.size.y / 4) +
                                           Input.mousePosition +  new Vector3(-90, 20);
        }
    }

    private void UpdateAvailability()
    {
        SetAppropriateColors();
    }
    
    public void SetForegroundColor(Color color) 
    {
        Image foregroundImage = transform.Find("SelectionImage").GetComponent<Image>();
        
        if (foregroundImage != null) 
        {
            foregroundImage.color = color;
        }
        
        CurrentColor = color;
    }

    public void OnButtonClicked()
    {
        if (CurrentColor == COLOR_AVAILABLE)
        {
            gameResources.ChangeCreditsCount(-trap.Cost);
            IncrementTrapByOne();
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

    private void IncrementTrapByOne()
    {
        switch (trap.TrapId)
        {
            case (int)TrapType.Molotov:
                gameResources.Molotovs++;
                break;
            case (int) TrapType.SpikeTrap:
                gameResources.SpikeTraps++;
                break;
            case (int) TrapType.SplashTrap:
                gameResources.SplashTraps++;
                break;
        }
    }
    
    private void SetAppropriateColors()
    {
        int currentCredits = gameResources.Credits;
        if (currentCredits >= trap.Cost)
        {
            SetForegroundColor(COLOR_AVAILABLE);
        }
        else
        {
            SetForegroundColor(COLOR_DISABLED);
        }
    }
}
