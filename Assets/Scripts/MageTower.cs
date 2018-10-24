using System.Collections.Generic;
using UnityEngine;

public class MageTower : MonoBehaviour
{
    public float range;
    public GameObject objectToSpawn;
    public GameObject[] bolts;

    public AudioSource audioSource;

    private List<GameObject> inRange;
    public int maxTargets;
    // Use this for initialization
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        inRange = new List<GameObject>();
        bolts = new GameObject[maxTargets];
        for (int i = 0; i < bolts.Length; i++)
        {
            bolts[i] = Instantiate(objectToSpawn, transform.position, transform.rotation);
            bolts[i].transform.GetChild(0).transform.position = transform.position;
            bolts[i].transform.GetChild(0).transform.position += new Vector3(0, 12, 0);
            bolts[i].SetActive(false);

            InvokeRepeating("FindTarget", 0f, 0.5f);
            InvokeRepeating("AttackEnemy", 0f, 0.5f);
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FindTarget()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject enemy in objects)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                inRange.Add(enemy);
            }
        }
    }

    private void AttackEnemy()
    {
        if (inRange.Count != 0)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (bolts.Length <= inRange.Count)
            {
                for (int i = 0; i < bolts.Length; i++)
                {
                    bolts[i].transform.GetChild(1).transform.position = inRange[i].transform.position;
                    bolts[i].SetActive(true);
                    inRange[i].SendMessage("DealDamage", 5.0);
                }
            }
            else
            {
                for (int i = 0; i < inRange.Count; i++)
                {
                    bolts[i].transform.GetChild(1).transform.position = inRange[i].transform.position;
                    bolts[i].SetActive(true);
                    inRange[i].SendMessage("DealDamage", 5.0);
                }
                for (int i = inRange.Count - 1; i < bolts.Length; i++)
                {
                    bolts[i].transform.GetChild(1).transform.position = inRange[0].transform.position;
                    inRange[0].SendMessage("DealDamage", 5.0);
                    bolts[i].SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < bolts.Length; i++)
            {
                bolts[i].SetActive(false);
            }
        }
        inRange.Clear();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
