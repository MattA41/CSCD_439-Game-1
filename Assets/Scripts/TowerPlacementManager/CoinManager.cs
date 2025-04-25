using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public Text coinText;
    public PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = playerManager.coins.ToString();
    }
    
    public void TowerPlaced()
    {
       playerManager.PlaceBasicTower();
    }
}
