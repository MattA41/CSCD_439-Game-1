using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerManager manager;
    bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;

        Screen.SetResolution(1920, 1080, false);

        QualitySettings.vSyncCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.isDead){
           isGameOver = true;
           Debug.Log("game is over");
           EndScreen();
        }
    }

   void EndScreen()
    {
        if (isGameOver)
        {
            SceneManager.LoadScene("Scenes/Menu/SelectScreen");
        }
    }


}
