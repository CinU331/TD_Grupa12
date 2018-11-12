using UnityEngine;

public class BuildingSpot : MonoBehaviour {

    public bool isOcuppied = false;
    public GameObject magicalTower;
    public GameObject currentTower;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateTower()
    {
        if (!isOcuppied)
        {
            currentTower = GameObject.Instantiate(magicalTower);
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
