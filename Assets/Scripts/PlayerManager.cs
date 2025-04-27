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
        if (!isDead && health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        health = 0;
        isDead = true;
        Debug.Log("Player Died!");
    }

    public bool TrySpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }


    public void PlaceBasicTower()
    {
        Debug.Log("Tower Placed!");
        coins = coins - 30;
    }
}
