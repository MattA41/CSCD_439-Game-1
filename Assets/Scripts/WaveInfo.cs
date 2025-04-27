using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveInfo : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // enemies to spawn in this wave
    public int enemyCount; // how many enemies 
    public float spawnInterval; // time between spawns
}
