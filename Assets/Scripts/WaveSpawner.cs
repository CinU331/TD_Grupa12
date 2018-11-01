using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public static int wave = 0;
    public static int numberOfWaves = 3;
    public static int aliveEnemies = 0; 
    public GameObject enemy;
    public GameObject boss;
    public Transform startGate;

    private bool waveSpawningInProgress = false;

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
        if (wave < numberOfWaves-1)
        {
            for (int i = 0; i < 10; i++)
            {

                SpawnEnemy(0);
                aliveEnemies++;
                yield return new WaitForSeconds(0.5f);
            }
        }
        else {
            SpawnEnemy(1);
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
                    GameObject gameObject = Instantiate(boss, startGate.position, transform.rotation);
                    gameObject.tag = "Respawn";
                    break;
                }
        }
    }
}
