using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        void QuitGame()
        {
            Application.Quit();
            Debug.Log("Exiting Game");
            //Just to make sure its working
        }
    }
}
