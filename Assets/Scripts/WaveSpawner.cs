using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int wave = 0;
    public static int numberOfWaves = 3;
    public static int aliveEnemies = 0;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject boss;
    public Transform startGate;

    private bool waveSpawningInProgress = false;
    private int choice;
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
            for (int i = 0; i < 10; i++)
            {
                choice = rnd.Next(2);
                SpawnEnemy(choice);
                aliveEnemies++;
                yield return new WaitForSeconds(0.5f);
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
