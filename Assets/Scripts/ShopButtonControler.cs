using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        GetComponent<Image>().color = Color.yellow;
    }

}
