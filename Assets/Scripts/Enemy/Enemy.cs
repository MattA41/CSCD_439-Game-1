using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5.0f;
    public PlayerManager manager; 
    public int health = 50;
    public GameObject[] waypoints;
    int currentWP = 0;
    public string TakeDamageType = "magic";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position,waypoints[currentWP].transform.position)< .1f)
            currentWP++;
        
        Vector3 newPos = Vector3.MoveTowards(this.transform.position, waypoints[currentWP].transform.position, speed * Time.deltaTime);
        this.transform.position = newPos;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health" + health);

        if (health <= 0)
        {
            Destroy(gameObject);
            CollectMoney(25);
        }
    }

    void CollectMoney(int amount)    //Adds Money when enemy is destroyed. You can change the param to any val.
    {
        if (health <= 0)
        {
            manager.coins = manager.coins + amount;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Goal" && manager.health != 0){
            manager.health = --manager.health;
            Debug.Log("goal reached " + manager.health);
            Destroy(this.gameObject);
        }
    }
}
