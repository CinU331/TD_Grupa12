using UnityEngine;
using UnityEngine.UI;

public class Enemies : MonoBehaviour {
    private Transform exitGate;
    private int numberOfWaypoint = 0;
    public float speed = 50f;
    public static int resources = 20;
    public float iMaxHp = 400;
    public float iCurrentHp;

    void Start () {
        exitGate = Waypoints.waypoints[0];
	iCurrentHp = iMaxHp;
    }
	
	void Update () {
        Vector3 lookDirection = (exitGate.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection);

        Vector3 directions = exitGate.position - transform.position;
        transform.Translate(directions.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, exitGate.position) <= 0.4f)
        {
            if (numberOfWaypoint == 28)
            {
                resources--;
            }
            GoToNextWaypoint();
        }
    }

    private void GoToNextWaypoint()
    {
        if (numberOfWaypoint >= Waypoints.waypoints.Length - 1)
        {
            WaveSpawner.aliveEnemies--;
            Destroy(gameObject);
            return;
        }

        numberOfWaypoint++;
        exitGate = Waypoints.waypoints[numberOfWaypoint];
    }

    public void DealDamage(float aValue)
    {
        iCurrentHp -= aValue;
        transform.Find("HealthBar").Find("Background").Find("Foreground").GetComponent<Image>().fillAmount = iCurrentHp / iMaxHp;  
        if (iCurrentHp <= 0)
        {
            Destroy(gameObject);
            if (WaveSpawner.aliveEnemies > 0)
                WaveSpawner.aliveEnemies--;
        }
        
    }
}
