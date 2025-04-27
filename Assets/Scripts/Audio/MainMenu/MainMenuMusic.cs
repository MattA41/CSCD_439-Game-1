using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    public AudioClip musicClip; // Assign in Inspector
    private AudioSource audioSource;
    private static MainMenuMusic instance;

    private string[] allowedScenes = { "MainMenu", "SelectScreen" }; // <-- Match your scene names!
    private float fadeOutTime = 2.0f; // Duration of fade in seconds

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 1.0f; // full volume
        audioSource.Play();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool shouldExist = false;

        foreach (string sceneName in allowedScenes)
        {
            if (scene.name == sceneName)
            {
                shouldExist = true;
                break;
            }
        }

        if (!shouldExist)
        {
            // Start fading out before destroying
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeOutTime;
            yield return null;
        }

        audioSource.Stop();

        // Give a small delay before destroy (fixes weird Unity load order problems)
        yield return new WaitForSecondsRealtime(0.1f);

        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
