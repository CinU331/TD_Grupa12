using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    private int wave = 0;
    public static int aliveEnemies = 0; 
    public GameObject enemy;
    public GameObject boss;
    public Transform startGate;

    void Update()
    {
        if (aliveEnemies == 0 && wave < 3)
        {
             StartCoroutine(SpawnWave());
             wave++;
        }
    }

    IEnumerator SpawnWave()
    {
        if (wave < 2)
        {
            for (int i = 0; i < 10; i++)
            {

                SpawnEnemy(0);
                aliveEnemies++;
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
            SpawnEnemy(1);

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
