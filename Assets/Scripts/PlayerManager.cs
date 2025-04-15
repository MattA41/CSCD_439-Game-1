using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int health = 50;
    public int coins = 100;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && health <= 0){

            health = 0;
            isDead = true;
        }
    }


    public void PlaceBasicTower()
    {
        Debug.Log("Tower Placed!");
        coins = coins - 30;
    }
}
