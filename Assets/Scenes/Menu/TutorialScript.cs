using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialScript : MonoBehaviour
{

    public void launchtutorial()
    {
        SceneManager.LoadScene("Scenes/tutorial");
    }
}
