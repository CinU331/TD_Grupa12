using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButtonControler : MonoBehaviour
{
    public static Color COLOR_SELECTED = new Color(95/255f, 95/255f, 255/255f, 20/255f);
    public static Color COLOR_AVAILABLE = new Color(0/255f, 185/255f, 35/255f, 10/255f);
    public static Color COLOR_DISABLED = new Color(255/255f, 30/255f, 0/255f, 10/255f);

    public Color CurrentColor { get; private set; }

    void Start()
    {
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
}
