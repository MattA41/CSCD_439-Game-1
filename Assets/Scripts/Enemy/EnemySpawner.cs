using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay = 3f;
    private float nextSpawnTime;
    public GameObject[] mapWayPoints;
    public PlayerManager pmanager;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, gameObject.transform);
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.waypoints = mapWayPoints;
        enemyScript.manager = pmanager;
    }
}
