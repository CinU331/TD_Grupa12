using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCredits : MonoBehaviour
{
    public TextMeshProUGUI creditsCount;
    private GameResources gameResources;

    // Use this for initialization
    void Start()
    {
        creditsCount = GetComponent<TextMeshProUGUI>();

        gameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        gameResources.CreditsChanged += UpdateCredits;

        UpdateCredits();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateCredits()
    {
        creditsCount.text = gameResources.Credits.ToString();
    }
}
