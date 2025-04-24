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
    //enemy stuff
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

    //wave stuff
    public bool IsWaves;
    public int waveNums = 10;
    public int enemyStartNum = 10;
    public int waveDelay = 30;
    public int enemyAdd = 3;
    private int currEnemyCount;
    public int currWave;
    private bool isSpawningWaves = false;
    //pause stuff
    private bool isPausedBetweenWaves = false;

    public Button roundButton;
    public Sprite playIcon;
    public Sprite pauseIcon;

    public enum GamePhase
    {
        BetweenWaves,
        Running,
        Paused
    }

    private GamePhase currentPhase = GamePhase.BetweenWaves;

    void Start()
    {
        currEnemyCount = enemyStartNum;
        Time.timeScale = 1f;
        SetRoundButtonIcon(playIcon);  // Start with play icon
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

    

        for (int i = 0; i < waveNums; i++)
        {   
            
            isPausedBetweenWaves = true;
            currentPhase = GamePhase.BetweenWaves;
            SetRoundButtonIcon(playIcon);
            yield return new WaitUntil(() => isPausedBetweenWaves == false);
    
            SetRoundButtonIcon(pauseIcon);
            currentPhase = GamePhase.Running;
            currWave = i + 1;
            Debug.Log("Wave " + currWave + " started");

            if (currWave <= 1)
            {
                for (int j = 0; j < currEnemyCount; j++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            else
            {
                int j = 0;
                while (j < currEnemyCount)
                {
                    for (int e = 0; e < enemyAtOnce; e++)
                    {
                        SpawnEnemy();
                        yield return new WaitForSeconds(1);
                        j++;
                    }

                }
            }


            Debug.Log("Wave " + currWave + " ended");
            if (currWave == 50 && EndGamePopup != null)    //Change currWave to desired end round
            {
                Time.timeScale = 0f;
                EndGamePopup.SetActive(true);

                continueFreePlayButton.onClick.RemoveAllListeners();
                returnToMenuButton.onClick.RemoveAllListeners();

                continueFreePlayButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    EndGamePopup.SetActive(false);
                    IsWaves = false; // switch to free play
                });

                returnToMenuButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Menu/MainMenu");
                });

                // Optional: skip remaining waves in test
                yield break;
            }


            currEnemyCount += enemyAdd;
        }
    }

    public void OnRoundButtonClick()
    {
        switch (currentPhase)
        {
            case GamePhase.BetweenWaves:
                isPausedBetweenWaves = false;
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

    void InstantiateEnemy(GameObject enemy)
    {
        GameObject createEnemy = (GameObject)Instantiate(enemy, gameObject.transform);
        var enemyScript = createEnemy.GetComponent<Enemy>();
        enemyScript.waypoints = mapWayPoints;
        enemyScript.manager = pmanager;
        enemyScript.health += enemyHealthAdd;
        enemyScript.speed += enemySpeedAdd;
        enemyScript.worth = enemyWorth;
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
}
