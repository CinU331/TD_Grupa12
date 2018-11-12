using System.Collections.Generic;
using UnityEngine;

public class CanonTower : MonoBehaviour
{
    private bool isShotInProgress;
    public float range = 20;
    public float splashRange = 5;
    public float turnSpeed = 10;
    public float ballSpeed = 25;

    private ParticleSystem directionalSmoke;
    private ParticleSystem smallExplosion;

    private Transform cannonBallTransform;
    private List<GameObject> inRange;

    private GameObject mockBall;
    public GameObject cannonBall;
    private GameObject target;
    private GameObject[] currentEnemies;


    // Use this for initialization
    private void Start()
    {
        mockBall = new GameObject();
        mockBall.transform.position = cannonBall.transform.position;

        mockBall.transform.parent = transform.GetChild(1);
        cannonBallTransform = transform.GetChild(1).GetChild(2);

        inRange = new List<GameObject>();
        InvokeRepeating("FindTarget", 0f, 0.05f);
        InvokeRepeating("UpdateRotation", 0f, 0.05f);
        InvokeRepeating("AttackEnemy", 0f, 4f);

        directionalSmoke = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mM = directionalSmoke.main;
        mM.startSize = 5;
        
        smallExplosion = transform.GetChild(3).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (target == null)
        {
            cannonBall.transform.position = mockBall.transform.position;
            isShotInProgress = false;
            return;
        }

        if (isShotInProgress && target != null)
        {
            smallExplosion.transform.position = mockBall.transform.position;
            smallExplosion.Play();

            if (Vector3.Distance(cannonBall.transform.position, target.transform.position) < 1)
            {
                directionalSmoke.transform.position = target.transform.position;
                directionalSmoke.Play();

                target.SendMessage("DealDamage", 600);

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Respawn");


                for (int i = 0; i < enemies.Length; i++)
                {
                    if (Vector3.Distance(enemies[i].transform.position, target.transform.position) < splashRange && enemies[i] != target)
                    {
                        enemies[i].SendMessage("DealDamage", 200);
                    }
                }

                cannonBall.transform.position = mockBall.transform.position;


                inRange.Clear();
                isShotInProgress = false;
                target = null;
                return;
            }

            Vector3 direction = target.transform.position - cannonBallTransform.position;
            float distance = ballSpeed * Time.deltaTime;
            cannonBallTransform.Translate(direction.normalized * distance, Space.World);
        }
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

        if (inRange.Count != 0)
        {
            target = inRange[0];
        }
        else
        {
            target = null;
        }
        currentEnemies = inRange.ToArray();
        inRange.Clear();
    }

    private void UpdateRotation()
    {
        if (target == null)
        {
            return;
        }

        transform.GetChild(1);
        Vector3 direction = target.transform.position - transform.GetChild(1).transform.position;
        Quaternion lookQuater = Quaternion.LookRotation(direction);

        Vector3 newRotation = Quaternion.Lerp(transform.GetChild(1).rotation, lookQuater, Time.deltaTime * turnSpeed).eulerAngles;
        transform.GetChild(1).rotation = Quaternion.Euler(0f, newRotation.y, 0f);
    }

    private void AttackEnemy()
    {

        if (target != null)
        {
            isShotInProgress = true;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Stop();
            audioSource.Play();
        }
        inRange.Clear();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
