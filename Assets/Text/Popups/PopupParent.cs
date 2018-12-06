using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupParent : MonoBehaviour {
    public static Popup popup;

    public static void Initialize(GameObject enemy)
    {
        //canvas = enemy.GetComponentInChildren<Canvas>();
        if (!popup)
            popup = Resources.Load<Popup>("PopUpParent");
    }

	public static void CreatePopup(string text, Transform location, Color color)
    {
        Canvas canvas = location.GetComponentInChildren<Canvas>();
        Popup instance = Instantiate(popup);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.localPosition = new Vector3(0,0,0);
        instance.SetText(text, color);
    }

    public static void CreatePopup(string text, Transform location)
    {
        Canvas canvas = location.GetComponentInChildren<Canvas>();
        Popup instance = Instantiate(popup);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.localPosition = new Vector3(0, 0, 0);
        instance.SetText(text);
    }
}
