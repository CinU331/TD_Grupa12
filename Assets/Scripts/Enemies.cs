using UnityEngine;

public class Enemies : MonoBehaviour {
    private Transform exitGate;
    private int numberOfWaypoint = 0;
    public float speed = 50f;
    public static int resources = 20;

    void Start () {
        exitGate = Waypoints.waypoints[0];
    }
	
	void Update () {
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
}
