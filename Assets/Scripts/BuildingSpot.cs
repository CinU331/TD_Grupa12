using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;

    public bool isOcuppied = false;
    public GameObject magicalTower;
    public GameObject cannonTower;
    public GameObject archerTower;
    private float defaultRange = 2.5f;

    public Light light;
    public GameObject lightHolder;

    public GameObject currentTower;
    private int currentTowerType;

    private LineRenderer line;
    private void Start()
    {
        lightHolder = new GameObject();
        Transform newTransform = lightHolder.GetComponent<Transform>();
        float oldX = GetComponent<Transform>().position.x;
        float oldY = GetComponent<Transform>().position.y;
        float oldZ = GetComponent<Transform>().position.z;

        Vector3 newPostion = new Vector3(oldX, oldY, oldZ);
        newTransform.position = newPostion;
        lightHolder.transform.position = newPostion;
        transform.position += new Vector3(0, 0.2f, 0);


        light = lightHolder.AddComponent<Light>();
        light.type = LightType.Spot;
        lightHolder.transform.Translate(0, 20, 0);
        lightHolder.transform.Rotate(90, 0, 0);
        light.intensity = 1000000;
        light.color = Color.red;
        light.range = 20.5f;
        light.enabled = false;
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
                        currentTowerType = 1;
                        break;
                    }
                case "CannonTowerItem":
                    {
                        currentTower = GameObject.Instantiate(cannonTower);
                        currentTowerType = 2;
                        break;
                    }
                case "ArcherTowerItem":
                    {
                        currentTower = GameObject.Instantiate(archerTower);
                        currentTowerType = 3;
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

        if (currentTower != null && currentTowerType == 1)
        {
            currentTower.SendMessage("StopAllAnimations");
        }
        currentTowerType = 0;
    }

    public void SetLightEnabled(bool aState)
    {
        light.enabled = aState;
    }

    public void SetLightRadiusDefault()
    {
        float expectedRadius = 5;
        float halvedRadius = expectedRadius / 2.0f;
        float tangensValue = halvedRadius / light.range;
        float computedAngle = Mathf.Atan(tangensValue) * Mathf.Rad2Deg;
        computedAngle *= 2;
        light.spotAngle = computedAngle;
    }

    public void SetLightRadiusExpanded()
    {
        if (currentTower != null)
        {
            

            switch (currentTowerType)
            {
                case 1:
                    {
                        float expectedRadius = currentTower.GetComponent<MageTower>().GetRange() * 2;
                        float tangensValue = expectedRadius / light.range;
                        float computedAngle = Mathf.Atan(tangensValue) * Mathf.Rad2Deg;
                        computedAngle *= 2;
                        light.spotAngle = computedAngle;
                        break;
                    }
                case 2:
                    {
                        float expectedRadius = currentTower.GetComponent<CanonTower>().GetRange() * 2;
                        float tangensValue = expectedRadius / light.range;
                        float computedAngle = Mathf.Atan(tangensValue) * Mathf.Rad2Deg;
                        computedAngle *= 2;
                        light.spotAngle = computedAngle;
                        break;
                    }
                case 3:
                    {
                        float expectedRadius = currentTower.GetComponent<ArcherTower>().GetRange() * 2;
                        float tangensValue = expectedRadius / light.range;
                        float computedAngle = Mathf.Atan(tangensValue) * Mathf.Rad2Deg;
                        computedAngle *= 2;
                        light.spotAngle = computedAngle;
                        break;
                    }
            }
        }

    }


}
