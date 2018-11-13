using UnityEngine;

public class BuildingSpot : MonoBehaviour
{

    public bool isOcuppied = false;
    public GameObject magicalTower;
    public GameObject cannonTower;
    public GameObject archerTower;

    public GameObject currentTower;

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void CreateTower(string aNameOfTower)
    {
        if (!isOcuppied)
        {
            switch (aNameOfTower)
            {
                case "MagicalTowerItem":
                    {
                        currentTower = GameObject.Instantiate(magicalTower);
                        break;
                    }
                case "CannonTowerItem":
                    {
                        currentTower = GameObject.Instantiate(cannonTower);
                        break;
                    }
                case "ArcherTowerItem":
                    {
                        currentTower = GameObject.Instantiate(archerTower);
                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            currentTower.transform.position = transform.position;
            Vector3 newScale = GetComponent<Collider>().transform.localScale;
            newScale.y = 20;
            GetComponent<Collider>().transform.localScale = newScale;
            isOcuppied = true;
        }

    }


    public void SellTower()
    {
        GameObject.Destroy(currentTower);
        Vector3 newScale = GetComponent<Collider>().transform.localScale;
        newScale.y = 0.2f;
        GetComponent<Collider>().transform.localScale = newScale;

        //DODAJ CZĘŚĆ jej kosztu
        isOcuppied = false;
    }
}
