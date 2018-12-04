using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int wave = 0;
    public static int numberOfWaves = 3;
    public static int aliveEnemies = 0;
    public static int timeToNextWave;
    public static float timeToNextWaveFloat;
    private float delay;
    private int spawnedEnemies;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject boss;
    public Transform startGate;

    private bool waveSpawningInProgress = false;
    private bool nextWaveClicked = false;
    private bool cameraChanged = false;
    System.Random rnd = new System.Random();
    public Button nextWaveButton;
    public Text winner;
    public Button mainMenu;
    public Button b_continue;

    public ChangingCamera changingCamera;
    
    public TextMeshProUGUI endWaveInfo;

    public BuildController buildController;
    public Canvas timeToNextWaveText;


    void Start()
    {
        wave = 0;
        numberOfWaves = 3;
        aliveEnemies = 0;
        nextWaveButton = GameObject.Find("NextWave").GetComponent<Button>();
        nextWaveButton.onClick.AddListener(() => NextWaveClicked());
        winner = GameObject.Find("Winner").GetComponent<Text>();
        mainMenu = GameObject.Find("MainMenuButton").GetComponent<Button>();
        mainMenu.onClick.AddListener(() => MainMenuClicked());
        b_continue = GameObject.Find("ContinueButton").GetComponent<Button>();
        b_continue.onClick.AddListener(() => ContinueClicked());
        timeToNextWaveText = GameObject.Find("TimeToNextWave").GetComponent<Canvas>();
        winner.gameObject.SetActive(false);
    }

    void NextWaveClicked()
    {
        nextWaveClicked = true;        
        changingCamera.SetActiveThirdCam();
        changingCamera.canChangeCamera = false;
        cameraChanged = false;
    }

    void MainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ContinueClicked()
    {
        mainMenu.gameObject.SetActive(false);
        b_continue.gameObject.SetActive(false);
        winner.gameObject.SetActive(false);
        winner.enabled = false;
        changingCamera.SetActiveThirdCam();
    }

    void Update()
    {
        if (Input.GetKeyDown("n") && nextWaveButton.gameObject.activeInHierarchy == true)
            NextWaveClicked();
        if (nextWaveClicked && !waveSpawningInProgress && aliveEnemies == 0 && wave < numberOfWaves)
        {
            waveSpawningInProgress = true;
            StartCoroutine(SpawnWave());
            wave++;
            nextWaveClicked = false;
            nextWaveButton.gameObject.SetActive(false);
            timeToNextWaveText.gameObject.SetActive(false);
        }
        else if (!nextWaveClicked && !waveSpawningInProgress && aliveEnemies == 0 && wave < numberOfWaves)
        {
            if (!cameraChanged)
            {
                changingCamera.SetActiveTacticalCam();
                timeToNextWaveFloat = 30f;
                cameraChanged = true;
            }

            timeToNextWaveText.gameObject.SetActive(true);
            Time.timeScale = 1.0f;
            timeToNextWaveFloat -= Time.deltaTime;
            timeToNextWave = Convert.ToInt32(timeToNextWaveFloat % 60);

            nextWaveButton.gameObject.SetActive(true);
            endWaveInfo.text = "Prepare to the next wave!";

            if (wave == 0)
            {
                endWaveInfo.text = "Prepare to the first wave!";
            }

            changingCamera.canChangeCamera = true;

            if (timeToNextWave < 0)
                NextWaveClicked();
        }

        if (aliveEnemies == 0 && wave == numberOfWaves)
        {
            winner.gameObject.SetActive(true);
            winner.gameObject.GetComponent<Text>().text = "You are the winner!";
            changingCamera.SetActiveTacticalCam();
        }

        if(aliveEnemies > 0)
        {
            endWaveInfo.text = " ";
        }
    }

    IEnumerator SpawnWave()
    {
        if (wave < numberOfWaves - 1)
        {
            for (int i = 0; i < 6; i++)
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
