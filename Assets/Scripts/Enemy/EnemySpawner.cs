using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("End Game Popup")]
    public GameObject EndGamePopup;
    public Button continueFreePlayButton;
    public Button returnToMenuButton;

    [Header("Enemy stuff")]
    public GameObject[] enemyPrefabs;
    public GameObject defaultEnemy;
    public float spawnDelay = 3.0f;
    public int enemyAtOnce = 5;
    private float nextSpawnTime;
    public GameObject[] mapWayPoints;
    public PlayerManager pmanager;
    public int enemyHealthAdd = 1;
    public float enemySpeedAdd = 0.5f;
    public int enemyWorth = 25;

    [Header("Wave stuff")]
    public List<WaveInfo> waves = new List<WaveInfo>();
    public bool IsWaves;
    public int waveNums = 10;
    public int enemyStartNum = 10;
    public int waveDelay = 30;
    public int enemyAdd = 3;
    private int currEnemyCount;
    public int currWave;
    private bool isSpawningWaves = false;

    [Header("Pause stuff")]
    // private bool isPausedBetweenWaves = false;

    public Button roundButton;
    public Sprite playIcon;
    public Sprite pauseIcon;

    public enum GamePhase
    {
        BetweenWaves,
        Running,
        Paused
    }

    [Header("tutorial stuff")]
    public bool isTutorial;

    private GamePhase currentPhase = GamePhase.BetweenWaves;

    void Start()
    {
        SetupWaves();

        currEnemyCount = enemyStartNum;
        Time.timeScale = 1f;
        SetRoundButtonIcon(playIcon);  // Start with play icon

        IsWaves = true;
        currentPhase = GamePhase.Running;
    }

    void Update()
    {
        if (IsWaves)
        {
            if (!isSpawningWaves)
            {
                StartCoroutine(WaveSpawner());
            }
        }
        else if (isTutorial)
        {
            // StartCoroutine(TutorialSpawner());
        }
        else
        {
            if (Time.time > nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnDelay;
            }
        }

    }

    IEnumerator WaveSpawner()
    {
        isSpawningWaves = true;

        for (int i = 0; i < waves.Count; i++)
        {
            currWave = i + 1;
            Debug.Log("Wave " + currWave + " started");

            WaveInfo currentWave = waves[i];

            for (int j = 0; j < currentWave.enemyCount; j++)
            {
                int randomIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
                GameObject enemyPrefab = currentWave.enemyPrefabs[randomIndex];

                SpawnEnemyType(enemyPrefab);
                yield return new WaitForSeconds(currentWave.spawnInterval);
            }

            Debug.Log("Wave " + currWave + " ended");

            if (currWave == waves.Count && EndGamePopup != null)
            {

                // Wait until all enemies are dead
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
                Time.timeScale = 0f;
                EndGamePopup.SetActive(true);

                continueFreePlayButton.onClick.RemoveAllListeners();
                returnToMenuButton.onClick.RemoveAllListeners();

                continueFreePlayButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    EndGamePopup.SetActive(false);
                    IsWaves = false;
                });

                returnToMenuButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Menu/MainMenu");
                });

                yield break;
            }

            // 💥 If you still want a small break between waves, you can do
            if (currWave != waves.Count)
            {
                yield return new WaitForSeconds(3f);
            }


        }

        isSpawningWaves = false;
    }

    // IEnumerator TutorialSpawner()
    // {
    //     isPausedBetweenWaves = true;
    //     currentPhase = GamePhase.BetweenWaves;
    //     SetRoundButtonIcon(playIcon);
    //     yield return new WaitUntil(() => isPausedBetweenWaves == false);

    //     SetRoundButtonIcon(pauseIcon);
    //     currentPhase = GamePhase.Running;

    //     //spawn default
    //     currWave = 1;
    //     Debug.Log("Wave " + currWave + " started");
    //     for (int i = 0; i <= 5; i++)
    //     {
    //         SpawnEnemyType(enemyPrefabs[1]);
    //         yield return new WaitForSeconds(spawnDelay);
    //     }
    //     yield return new WaitForSeconds(waveDelay);

    //     //spawn fast
    //     currWave = 2;
    //     Debug.Log("Wave " + currWave + " started");
    //     for (int i = 0; i <= 4; i++)
    //     {
    //         SpawnEnemyType(enemyPrefabs[2]);
    //         yield return new WaitForSeconds(spawnDelay);
    //     }
    //     yield return new WaitForSeconds(waveDelay);
    //     //spawn slow
    //     currWave = 3;
    //     Debug.Log("Wave " + currWave + " started");
    //     for (int i = 0; i <= 3; i++)
    //     {
    //         SpawnEnemyType(enemyPrefabs[3]);
    //         yield return new WaitForSeconds(spawnDelay);
    //     }
    //     isTutorial = false;
    //     IsWaves = true;

    // }

    public void OnRoundButtonClick()
    {
        switch (currentPhase)
        {
            case GamePhase.BetweenWaves:
                // isPausedBetweenWaves = false;
                currentPhase = GamePhase.Running;
                Time.timeScale = 1f;
                SetRoundButtonIcon(pauseIcon);
                break;

            case GamePhase.Running:
                Time.timeScale = 0f;
                currentPhase = GamePhase.Paused;
                SetRoundButtonIcon(playIcon);
                break;

            case GamePhase.Paused:
                Time.timeScale = 1f;
                currentPhase = GamePhase.Running;
                SetRoundButtonIcon(pauseIcon);
                break;
        }

        Debug.Log("Round button clicked. New state: " + currentPhase);
    }

    void SetRoundButtonIcon(Sprite icon)
    {
        if (roundButton != null && icon != null)
        {
            Image img = roundButton.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = icon;
            }
        }
    }

    void SpawnEnemy()
    {
        if (currWave <= 1)
        {
            InstantiateEnemy(defaultEnemy);
        }
        else
        {
            var randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            InstantiateEnemy(enemyPrefabs[randomIndex]);
        }

    }

    void SpawnEnemyType(GameObject enemy)
    {
        InstantiateEnemy(enemy);
    }

    void InstantiateEnemy(GameObject enemy)
    {
        GameObject createEnemy = (GameObject)Instantiate(enemy, gameObject.transform);
        var enemyScript = createEnemy.GetComponent<Enemy>();
        enemyScript.waypoints = mapWayPoints;
        enemyScript.manager = pmanager;
        enemyScript.health += enemyHealthAdd;
        enemyScript.speed += enemySpeedAdd;
    }


    public void InGameMenuClick()
    {
        switch (currentPhase)
        {


            case GamePhase.Running:
                Time.timeScale = 0f;
                currentPhase = GamePhase.Paused;
                SetRoundButtonIcon(playIcon);
                break;

                //case GamePhase.Paused:
                //    Time.timeScale = 1f;
                //    currentPhase = GamePhase.Running;
                //    SetRoundButtonIcon(pauseIcon);
                //    break;
        }

    }

    private void SetupWaves()
    {
        waves = new List<WaveInfo>();

        waves.Add(new WaveInfo // Wave 1
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[0] }, // Normal enemy
            enemyCount = 10,
            spawnInterval = 2f
        });

        waves.Add(new WaveInfo // Wave 2
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[0] }, // Normal enemy
            enemyCount = 12,
            spawnInterval = 1.8f
        });

        waves.Add(new WaveInfo // Wave 3
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[1], enemyPrefabs[0] }, // Fast + Normal
            enemyCount = 14,
            spawnInterval = 1.5f
        });

        waves.Add(new WaveInfo // Wave 4
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[2], enemyPrefabs[0] }, // Slow + Normal
            enemyCount = 10,
            spawnInterval = 1.8f
        });

        waves.Add(new WaveInfo // Wave 5
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[1] }, // Fast only
            enemyCount = 15,
            spawnInterval = 1f
        });

        waves.Add(new WaveInfo // Wave 6
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[2], enemyPrefabs[0] }, // Slow + Normal
            enemyCount = 16,
            spawnInterval = 1.5f
        });

        waves.Add(new WaveInfo // Wave 7
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[0] }, // Normal
            enemyCount = 25,
            spawnInterval = 1.2f
        });

        waves.Add(new WaveInfo // Wave 8
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[1], enemyPrefabs[2] }, // Fast + Slow
            enemyCount = 25,
            spawnInterval = 1f
        });

        waves.Add(new WaveInfo // Wave 9
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[1], enemyPrefabs[0] }, // Fast swarm
            enemyCount = 30,
            spawnInterval = 0.8f
        });

        waves.Add(new WaveInfo // Wave 10
        {
            enemyPrefabs = new GameObject[] { enemyPrefabs[2] }, // Slow tank wave
            enemyCount = 15,
            spawnInterval = 1.5f
        });
    }
}
