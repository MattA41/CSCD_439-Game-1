using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerManager manager;
    public EnemyScript enemy;


    bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.isDead){
           isGameOver = true;
           Debug.Log("game is over");
        }
    }


}
