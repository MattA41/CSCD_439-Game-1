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
    private int lastWave = -1;

    private void Update()
    {
        if (waveText != null && enemySpawner != null)
        {
            if (enemySpawner.currWave != lastWave)
            {
                lastWave = enemySpawner.currWave;
                waveText.text = "Wave: " + lastWave.ToString();
            }
        }
    }



}
