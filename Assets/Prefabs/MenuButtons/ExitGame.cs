using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Exit button clicked. Quitting game...");
        Application.Quit();

        // For editor testing

    }
}
