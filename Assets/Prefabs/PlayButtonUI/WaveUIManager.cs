using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIManager : MonoBehaviour
{
    public Text waveText;
    public PlayerManager playerManager;
    public EnemySpawner enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = "Wave: " + enemySpawner.currWave.ToString();
    }

 
}
