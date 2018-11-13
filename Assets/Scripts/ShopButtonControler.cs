using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButtonControler : MonoBehaviour
{

    public static string towerButtonClicked = "";

    public void SetTowerButton()
    {
        towerButtonClicked = EventSystem.current.currentSelectedGameObject.name;
    }

}
