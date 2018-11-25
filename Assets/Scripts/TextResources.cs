using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextResources : MonoBehaviour {
    public TextMeshProUGUI resourcesCount;

    private GameResources gameResources;

    // Use this for initialization
    void Start()
    {
        resourcesCount = GetComponent<TextMeshProUGUI>();

        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        gameResources.ResourcesChanged += UpdateResources;

        UpdateResources();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateResources() 
    {
        resourcesCount.text = gameResources.Resources.ToString();
    }
}
