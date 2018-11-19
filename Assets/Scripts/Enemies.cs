﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct DamageParameters
{
    public float damageAmount;
    public float slowDownFactor;
    public float duration;
}


public class Enemies : MonoBehaviour
{
    private Transform exitGate;
    private int numberOfWaypoint = 0;
    public float speed = 50f;
    public static int resources = 5;
    public static int credits = 10;
    public float iMaxHp = 400;
    public float iCurrentHp;

    private bool isAlive;


    float slowDownFactor = 1;
    bool isSlowed = false;
    float duration;
    float startTime;
    float endTime;

    void Start()
    {
        exitGate = Waypoints.waypoints[0];
        iCurrentHp = iMaxHp;
        isAlive = true;
    }

    void Update()
    {

        if (isSlowed)
        {
            endTime = Time.time;
            if ((endTime - startTime) >= duration)
            {
                isSlowed = false;
                slowDownFactor = 1;
            }
        }

        Vector3 lookDirection = (exitGate.position - transform.position).normalized;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        Vector3 directions = exitGate.position - transform.position;
        transform.Translate(directions.normalized * speed * slowDownFactor * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, exitGate.position) <= 0.4f)
        {
            if (numberOfWaypoint == 40)
            {
                if (GameObject.Find("OrcHB(Clone)"))
                {
                    resources -= 4;
                }
                else
                    resources--;
            }
            if (resources <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            GoToNextWaypoint();
        }
    }

    private void GoToNextWaypoint()
    {
        if (numberOfWaypoint >= Waypoints.waypoints.Length - 1)
        {
            if (GameObject.Find("OrcHB(Clone)"))
            {
                credits -= 20;
            }
            else
                credits -= 10;
            WaveSpawner.aliveEnemies--;
            Destroy(gameObject);
            return;
        }

        numberOfWaypoint++;
        exitGate = Waypoints.waypoints[numberOfWaypoint];
    }

    public void DealDamage(DamageParameters damageParameters)
    {
        if (!isSlowed)
        {
            duration = damageParameters.duration;
            slowDownFactor = damageParameters.slowDownFactor;
            isSlowed = true;
            startTime = Time.time;
        }
        iCurrentHp -= damageParameters.damageAmount;
        transform.Find("HealthBar").Find("Background").Find("Foreground").GetComponent<Image>().fillAmount = iCurrentHp / iMaxHp;
        if (iCurrentHp <= 0 && isAlive)
        {
            isAlive = false;
            if (GameObject.Find("OrcHB(Clone)"))
            {
                credits += 87 - numberOfWaypoint;
            }
            else
                credits += (87 - numberOfWaypoint) / 10;
            Destroy(gameObject);
            if (WaveSpawner.aliveEnemies > 0)
                WaveSpawner.aliveEnemies--;
        }
    }
}
