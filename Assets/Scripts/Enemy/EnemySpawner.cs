using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //prfab for spawning enemys
    public float spawnDelay = 3.0f; //delay inbetween enemy spawns
    private float nextSpawnTime; //time when next enemy will spawn
    //enemy vars
    public GameObject[] mapWayPoints; //waypoints for enemy navigation
    public PlayerManager pmanager; //player manager for enemy to affect health
    public int enemyHealth = 100; //holds the health value for enemys
    public int enemySpeed = 5; //hold the speed value for enemys
    //wave management
    public bool IsWaves; // If the level will have waves set to true
    public int waveNums = 10; //number of waves for the level
    public int enemyStartNum = 10; //number of enemys at start
    public int currWave; //the current waves
    public int waveDelay = 30; //delay inbetween waves in seconds default is 30 seconds
    public int enemyAdd = 3; //number of enemys to add at end of wave
    private int currEnemyCount ; //used to add enemys
    private bool isSpawningWaves = false;

    //Start is called before the first frame update
    void Start()
    {
        currEnemyCount = enemyStartNum;
        
    }

    //Update is called once per frame
    void Update()
    {
        if (IsWaves)
        {
            if(!isSpawningWaves)
            {
                StartCoroutine(WaveSpawner());
            }
            
        }else
        {
            if (Time.time > nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnDelay;
            }
        }

        
    }
    IEnumerator WaveSpawner() //spawn waves with a wait time without freezing the whole game
    {
        isSpawningWaves = true;

        for (int i = 0; i < waveNums; i++)
        {
            currWave = i + 1;
            Debug.Log("Wave " + currWave + " started");

            // Spawn all enemies in the wave
            for (int j = 0; j < currEnemyCount; j++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            Debug.Log("Wave " + currWave + " ended");
            currEnemyCount += enemyAdd;
            Debug.Log("Current enemy count: " + currEnemyCount);

            // Wait before the next wave starts
            yield return new WaitForSeconds(waveDelay);
        }

        Debug.Log("All waves complete");
        isSpawningWaves = false;
    }
    //spawns enemy and hands in values
    void SpawnEnemy()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, gameObject.transform);
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.waypoints = mapWayPoints;
        enemyScript.manager = pmanager;
        enemyScript.health = enemyHealth;
        enemyScript.speed = enemySpeed;
    }

    
}
