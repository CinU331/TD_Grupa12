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
    private float defaultRange = 2.5f;
    public List<GameObject> mockRocks;
    private List<GameObject> spawnedRocks;
    public GameObject currentTower;

    private LineRenderer line;
    private void Start()
    {
        rockPrefabs = new[] { rock1, rock2, rock3, rock4 };
        spawnedRocks = new List<GameObject>();
        mockRocks = new List<GameObject>();
    }

    public bool IsOccupied() 
    {
        return isOccupied;
    }

    public void CreateTower(GameObject tower)
    {
        if (!isOccupied)
        {
            currentTower = tower;
            currentTower.transform.position = transform.position;

            isOccupied = true;
        }

    }

    public string SellTower()
    {
        if (currentTower != null)
        {
            AbstractTower tower = currentTower.GetComponent<AbstractTower>();
            string soldTowerName = tower.TowerIdentificator;

            Destroy(currentTower);
            isOccupied = false;

            if (currentTower != null)
            {
                currentTower.SendMessage("StopAllAnimations");
            }

            return soldTowerName;
        }
        else
        {
            return "";
        }
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
            radius = 0;
        }
        else
        {
            radius = currentTower.GetComponent<AbstractTower>().range;
            rockColor = currentTower.GetComponent<AbstractTower>().rockColor;
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


    public void SpawnRocksMock(GameObject tower)
    {
        if(mockRocks.Count == 0 && isOccupied == false)
        {
            var radius = tower.GetComponent<AbstractTower>().range;
            var rockColor = tower.GetComponent<AbstractTower>().rockColor;

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
    
    public void SetNotOccupiedVisible(bool aState)
    {
        if(!isOccupied)
        {
            GetComponent<MeshRenderer>().enabled = aState;
            foreach(GameObject rock in spawnedRocks)
            {
                rock.GetComponent<MeshCollider>().enabled = aState;
                rock.GetComponent<MeshRenderer>().enabled = aState;
            }
        }
    }

    public void SetOccupiedVisible(bool aState)
    {
        if (isOccupied)
        {
            GetComponent<MeshRenderer>().enabled = false;
            foreach (GameObject rock in spawnedRocks)
            {
                rock.GetComponent<MeshCollider>().enabled = false;
                rock.GetComponent<MeshRenderer>().enabled = aState;
            }
        }
    }

    public void DestroyMockRocks()
    {
        if(mockRocks.Count != 0)
        {
            for(int i = 0; i < mockRocks.Count; i++)
            {
                Destroy(mockRocks[i]);
            }
            mockRocks.Clear();
        }
    }

}