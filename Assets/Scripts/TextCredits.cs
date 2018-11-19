using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCredits : MonoBehaviour
{
    public TextMeshProUGUI creditsCount;

    // Use this for initialization
    void Start()
    {
        creditsCount = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        creditsCount.text = Enemies.credits.ToString();
    }
}
