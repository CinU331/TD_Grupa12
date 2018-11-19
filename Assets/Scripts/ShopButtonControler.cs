using UnityEngine;
using UnityEngine.EventSystems;

public class ShopButtonControler : MonoBehaviour
{

    public static string towerButtonClicked = "";

    void Start()
    {
        towerButtonClicked = ""; 
    }

    public void SetTowerButton()
    {
        towerButtonClicked = EventSystem.current.currentSelectedGameObject.name;
    }

}
