using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemySpawner: MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject[] mapWayPoints;
    public float spawnInterval = 5f;

    private float timer = 0f;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.waypoints =  mapWayPoints;
    }
}
