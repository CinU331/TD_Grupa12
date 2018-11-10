using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildController : MonoBehaviour
    {
        public GameObject shopPanel;
        public GameObject camera;
        List<GameObject> buildingSpots;

        public Boolean IsPaused;
        private int build = 1;



        // Use this for initialization
        private void Start()
        {
            buildingSpots = new List<GameObject>();
            GameObject spotsSource = GameObject.Find("BuildingSpots");
            int i = 0;
            foreach(Transform transform in spotsSource.transform)
            {
                buildingSpots.Add(transform.gameObject);
                buildingSpots[i].tag = "BuildingSpot";

                Light lightComp = buildingSpots[i].AddComponent<Light>();
                lightComp.color = Color.green;
                lightComp.range = 5;
                lightComp.intensity = 100;
                lightComp.enabled = false;
                buildingSpots[i].transform.position += new Vector3(0, 2, 0);
                i++;
            }
            shopPanel.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            Boolean change = false;
            if (camera.GetComponent<Camera>().enabled == true && Input.GetKeyDown("b") && (build == 1) && !change)
            {
                build = 2;
                StartBuild();
                change = true;
            }
            if (camera.GetComponent<Camera>().enabled == true && Input.GetKeyDown("b") && (build == 2) && !change)
            {
                build = 1;
                StopBuild();
                change = true;
            }
            if (camera.GetComponent<Camera>().enabled == false && !change)
            {
                build = 1;
                StopBuild();
            }

            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject != null)
                    {
                        Debug.Log(hit.transform.gameObject.name);
                    }
                }
            }
        }

        public void StartBuild()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            IsPaused = true;

            shopPanel.SetActive(true);

            foreach (GameObject buildingSpot in buildingSpots)
            {

                buildingSpot.GetComponent<Collider>().enabled = true;
                buildingSpot.GetComponent<Light>().enabled = true;

            }
        }

        public void StopBuild()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            IsPaused = false;

            shopPanel.SetActive(false);

            foreach(GameObject buildingSpot in buildingSpots)
            {
                buildingSpot.GetComponent<Collider>().enabled = false;
                buildingSpot.GetComponent<Light>().enabled = false;
            }
        }
    }
}
