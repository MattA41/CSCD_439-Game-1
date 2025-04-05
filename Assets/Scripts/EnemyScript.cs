using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;
    public float speed = 10.0f;
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position,waypoints[currentWP].transform.position)< .1f)
            currentWP++;
        
        Vector3 newPos = Vector3.MoveTowards(this.transform.position, waypoints[currentWP].transform.position, speed * Time.deltaTime);
        this.transform.position = newPos;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Goal"){
            //manager.health = --manager.health;
            Debug.Log("goal reached " + manager.health);
            Destroy(this.gameObject);
        }
    }
}
