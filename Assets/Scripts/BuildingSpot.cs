using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;
    private GameObject[] rockPrefabs;


    public bool isOccupied = false;
    public GameObject magicalTower;
    public GameObject cannonTower;
    public GameObject archerTower;
    private float defaultRange = 2.5f;
    public List<GameObject> mockRocks;
    private List<GameObject> spawnedRocks;
    public GameObject currentTower;
    private int currentTowerType;

    private LineRenderer line;
    private void Start()
    {
        rockPrefabs = new GameObject[] { rock1, rock2, rock3, rock4 };
        spawnedRocks = new List<GameObject>();
        mockRocks = new List<GameObject>();
        SpawnRocks();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void CreateTower(string aNameOfTower)
    {
        if (!isOccupied)
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
            isOccupied = true;
        }

    }


    public void SellTower()
    {
        GameObject.Destroy(currentTower);
        Vector3 newScale = GetComponent<Collider>().transform.localScale;
        newScale.y = 0.2f;
        GetComponent<Collider>().transform.localScale = newScale;

        //DODAJ CZĘŚĆ jej kosztu
        isOccupied = false;

        if (currentTower != null && currentTowerType == 1)
        {
            currentTower.SendMessage("StopAllAnimations");
        }
        currentTowerType = 0;
    }

    public void SpawnRocks()
    {
        foreach (GameObject rock in spawnedRocks)
        {
            Destroy(rock);
        }
        spawnedRocks.Clear();
        Color rockColor = new Color(255, 255, 0);
        float radius;
        if (isOccupied == false)
        {
            radius = defaultRange;
        }
        else
        {
            switch (currentTowerType)
            {
                case 1:
                    {
                        radius = currentTower.GetComponent<MageTower>().GetRange();
                        rockColor = Color.blue;
                        break;
                    }
                case 2:
                    {
                        radius = currentTower.GetComponent<CanonTower>().GetRange();
                        rockColor = Color.black;
                        break;
                    }
                case 3:
                    {
                        radius = currentTower.GetComponent<ArcherTower>().GetRange();
                        rockColor = Color.magenta;
                        break;
                    }
                default:
                    {
                        radius = 0;
                        break;
                    }
            }
        }
        if (radius != 0)
        {
            int numObjects = (int)(4 * Mathf.PI * radius);

            Vector3 center = transform.position + new Vector3(0, 0.2f, 0);
            for (int i = 0; i < numObjects; i++)
            {
                Vector3 pos;
                float ang = i * (360.0f / numObjects);
                pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.y = center.y;
                pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);



                GameObject prefab = rockPrefabs[(int)Mathf.Round(Random.Range(0, 3))];
                GameObject newRock = Instantiate(prefab, pos, rot);
                newRock.GetComponent<MeshRenderer>().material.color = rockColor;

                if (radius != defaultRange)
                {
                    newRock.transform.localScale = new Vector3(0.1f, 0.4f, 0.01f);
                    newRock.GetComponent<MeshCollider>().enabled = false;
                    newRock.GetComponent<Collider>().enabled = false;

                    newRock.GetComponent<MeshRenderer>().material.color = rockColor;
                }
                else
                {
                    newRock.GetComponent<MeshCollider>().enabled = true;
                }
                newRock.GetComponent<MeshRenderer>().enabled = true;
                spawnedRocks.Add(newRock);
            }
        }
    }


    public void SpawnRocksMock(int typeVal)
    {
        if(typeVal == 0)
        {
            foreach (GameObject rock in mockRocks)
            {
                Destroy(rock);
            }
            mockRocks.Clear();
        }
        else if(mockRocks.Count == 0)
        {
            
            Color rockColor = new Color(255, 255, 0);
            float radius;


            switch (typeVal)
            {
                case 1:
                    {
                        radius = magicalTower.GetComponent<MageTower>().GetRange();
                        rockColor = Color.blue;
                        break;
                    }
                case 2:
                    {
                        radius = cannonTower.GetComponent<CanonTower>().GetRange();
                        rockColor = Color.black;
                        break;
                    }
                case 3:
                    {
                        radius = archerTower.GetComponent<ArcherTower>().GetRange();
                        rockColor = Color.magenta;
                        break;
                    }
                default:
                    {
                        radius = 0;
                        break;
                    }

            }
            if (radius != 0)
            {
                int numObjects = (int)(4 * Mathf.PI * radius);

                Vector3 center = transform.position + new Vector3(0, 0.2f, 0);
                for (int i = 0; i < numObjects; i++)
                {
                    Vector3 pos;
                    float ang = i * (360.0f / numObjects);
                    pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                    pos.y = center.y;
                    pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

                    Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);



                    GameObject prefab = rockPrefabs[(int)Mathf.Round(Random.Range(0, 3))];
                    GameObject newRock = Instantiate(prefab, pos, rot);
                    newRock.GetComponent<MeshRenderer>().material.color = rockColor;

                    if (radius != defaultRange)
                    {
                        newRock.transform.localScale = new Vector3(0.15f, 0.4f, 0.15f);
                        newRock.GetComponent<MeshCollider>().enabled = false;
                        newRock.GetComponent<Collider>().enabled = false;

                        newRock.GetComponent<MeshRenderer>().material.color = rockColor;
                    }
                    else
                    {
                        newRock.GetComponent<MeshCollider>().enabled = true;
                    }
                    newRock.GetComponent<MeshRenderer>().enabled = true;
                    mockRocks.Add(newRock);
                }
            }
        }

    }

    public void SetNotOccupiedVisible(bool state)
    {
        if(!isOccupied)
        {
            foreach(GameObject rock in spawnedRocks)
            {
                rock.GetComponent<MeshCollider>().enabled = state;
                rock.GetComponent<MeshRenderer>().enabled = state;
            }
        }
    }
}