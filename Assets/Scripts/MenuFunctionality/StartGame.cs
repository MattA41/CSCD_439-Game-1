using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartGame : MonoBehaviour
{
    public AudioClip clickSound;
    public string sceneToLoad;
    private AudioSource audioSource;

    public float delayBeforeLoad = 0.3f; // delay to allow sound to play
   
    private void Start()
    {
        Debug.Log("SceneLoaderWithSound: Start() called"); // ADD THIS

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;
        audioSource.volume = 0.7f;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }


    private void OnButtonClicked()
    {
        Debug.Log("Button was clicked!"); // <-- ADD THIS

        audioSource.Play();
        StartCoroutine(LoadSceneAfterDelay());
    }


    private IEnumerator LoadSceneAfterDelay()
    {
        Debug.Log("Trying to load scene: " + sceneToLoad);

        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(sceneToLoad);

    }
}

