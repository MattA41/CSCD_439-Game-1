using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //prfab for spawning enemys
    public float spawnDelay = 3f; //delay inbetween enemyies
    private float nextSpawnTime; //time when next enemy will spawn
    //enemy vars
    public GameObject[] mapWayPoints; //waypoints for enemy navigation
    public PlayerManager pmanager; //player manager for enemy to affect health
    public int enemyHealth; //holds the health value for enemys
    public int enemySpeed; //hold the speed value for enemys
    //wave management
    public bool IsWaves; // If the level will have waves set to true
    public int waveNums = 10; //number of waves for the level
    public int enemyStartNum = 10; //number of enemys at start
    public int currWave; //the current waves
    public int waveDelay = 300000; //delay inbetween waves in milliseconds default is 30 seconds
    private int currEnemyCount ; //used to add enemys 
    public int enemyAdd = 3; //number of enemys to add at end of wave

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
            for (int i = 0; i <= waveNums; i++)
            {
                Debug.Log("wave " + currWave + "started")
                WaveSpawn();
                Debug.Log("wave " + currWave + "ended")
                currEnemyCount = currEnemyCount + enemyAdd;
                Thread.Sleep(waveDelay);
            }
            
        }

        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
    //spawns enemy and hands in values
    void SpawnEnemy()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, gameObject.transform);
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.waypoints = mapWayPoints;
        enemyScript.manager = pmanager;
    }

    void WaveSpawn() //spawns enemies untill wave count
    {
        int i = 0;
        while (i <= currEnemyCount)
        {
            if (Time.time > nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnDelay;
                i++;
            }
        }
    }
}
