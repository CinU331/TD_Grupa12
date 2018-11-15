using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int wave = 0;
    public static int numberOfWaves = 3;
    public static int aliveEnemies = 0;
    private float delay;
    private int spawnedEnemies;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject boss;
    public Transform startGate;

    private bool waveSpawningInProgress = false;
    System.Random rnd = new System.Random();

    void Update()
    {
        if (aliveEnemies == 0 && !waveSpawningInProgress && wave < numberOfWaves)
        {
            waveSpawningInProgress = true;
            StartCoroutine(SpawnWave());
            wave++;
        }
    }

    IEnumerator SpawnWave()
    {
        if (wave < numberOfWaves - 1)
        {
            for (int i = 0; i < 5; i++)
            {
                if (spawnedEnemies < 5)
                {
                    SpawnEnemy(0);
                    spawnedEnemies++;
                }
                else
                    SpawnEnemy(1);
                aliveEnemies++;
                delay = rnd.Next(15, 40) / 10;
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            SpawnEnemy(2);
            aliveEnemies++;
        }

        waveSpawningInProgress = false;
    }

    private void SpawnEnemy(int enemyType)
    {
        switch (enemyType)
        {
            case 0:
                {
                    GameObject gameObject = Instantiate(enemy, startGate.position, transform.rotation);
                    gameObject.tag = "Respawn";
                    break;
                }
            case 1:
                {
                    GameObject gameObject = Instantiate(enemy2, startGate.position, transform.rotation);
                    gameObject.tag = "Respawn";
                    break;
                }
            case 2:
                {
                    GameObject gameObject = Instantiate(boss, startGate.position, transform.rotation);
                    gameObject.tag = "Respawn";
                    break;
                }
        }
    }
}
