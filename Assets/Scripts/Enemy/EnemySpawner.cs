using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay = 3.0f;
    private float nextSpawnTime;

    public GameObject[] mapWayPoints;
    public PlayerManager pmanager;
    public int enemyHealth = 100;
    public int enemySpeed = 5;

    public bool IsWaves;
    public int waveNums = 10;
    public int enemyStartNum = 10;
    public int waveDelay = 30;
    public int enemyAdd = 3;

    private int currEnemyCount;
    private int currWave;
    private bool isSpawningWaves = false;
    private bool isPausedBetweenWaves = false;

    public GameObject roundToggleButton; // Drag your UI button here in Inspector

    void Start()
    {
        currEnemyCount = enemyStartNum;
        if (roundToggleButton != null)
            roundToggleButton.SetActive(false);
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

        // Pause before first wave
        isPausedBetweenWaves = true;
        roundToggleButton.SetActive(true);
        Debug.Log("Waiting to start first wave...");
        yield return new WaitUntil(() => isPausedBetweenWaves == false);
        roundToggleButton.SetActive(false);

        for (int i = 0; i < waveNums; i++)
        {
            currWave = i + 1;
            Debug.Log("Wave " + currWave + " started");

            for (int j = 0; j < currEnemyCount; j++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            Debug.Log("Wave " + currWave + " ended");
            currEnemyCount += enemyAdd;

            if (i < waveNums - 1)
            {
                isPausedBetweenWaves = true;
                roundToggleButton.SetActive(true);
                Debug.Log("Waiting for player to start next wave...");
                yield return new WaitUntil(() => isPausedBetweenWaves == false);
                roundToggleButton.SetActive(false);
            }
        }

        Debug.Log("All waves complete");
        isSpawningWaves = false;
    }

    public void ContinueToNextWave()
    {
        Debug.Log("Continue button clicked!");
        isPausedBetweenWaves = false;
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.waypoints = mapWayPoints;
        enemyScript.manager = pmanager;
        enemyScript.health = enemyHealth;
        enemyScript.speed = enemySpeed;
    }
}
