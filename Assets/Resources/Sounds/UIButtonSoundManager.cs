using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIButtonSoundManager : MonoBehaviour
{
    public string clickSoundResourcePath = "Sounds/ButtonClick";

    private AudioClip clickSound;
    private AudioSource audioSource;
    private static UIButtonSoundManager instance;
    private List<Button> hookedButtons = new List<Button>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupAudioSource();
        LoadClickSound();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f;
    }

    private void LoadClickSound()
    {
        if (string.IsNullOrEmpty(clickSoundResourcePath))
            clickSoundResourcePath = "Sounds/ButtonClick";

        clickSound = Resources.Load<AudioClip>(clickSoundResourcePath);

        if (clickSound == null)
            Debug.LogWarning("UIButtonSoundManager: Click sound not found at Resources/" + clickSoundResourcePath);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("UIButtonSoundManager: Scene Loaded: " + scene.name);

        SetupAudioSource(); // Ensure AudioSource still exists
        hookedButtons.Clear();
        AttachClickSoundsToButtons();
    }

    private void Update()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("UIButtonSoundManager: AudioSource missing, recreating...");
            SetupAudioSource();
        }

        AttachClickSoundsToButtons();
    }

    private void AttachClickSoundsToButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button button in allButtons)
        {
            if (!hookedButtons.Contains(button))
            {
                button.onClick.AddListener(PlayClickSound);
                hookedButtons.Add(button);
            }
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("UIButtonSoundManager: Missing audioSource or clickSound!");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
