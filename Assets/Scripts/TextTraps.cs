using Assets.Scripts;
using TMPro;
using UnityEngine;

public class TextTraps : MonoBehaviour {
    public TextMeshProUGUI TrapsCount;

    private GameResources gameResources;
    public GameObject RepresentedTrap;
    private AbstractTrap trap;

    void Start()
    {
//        TrapsCount = GetComponent<TextMeshProUGUI>();
        trap = RepresentedTrap.GetComponent<AbstractTrap>();

        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        gameResources.TrapsChanged += UpdateTraps;

        UpdateTraps();
    }

    private void UpdateTraps()
    {
        string countText = "";
        
        switch (trap.TrapId)
        {
            case (int) TrapType.SpikeTrap:
                countText = gameResources.SpikeTraps.ToString();
                break;
            case (int) TrapType.SplashTrap:
                countText = gameResources.SplashTraps.ToString();
                break;
        }
        
        TrapsCount.text = countText;
    }
}
