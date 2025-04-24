using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsMenuPanel;

    [Header("Options UI")]
    public Slider volumeSlider;
    public Button saveButton;
    public Button loadButton;
    public Button exitButton;

    void Start()
    {
        optionsMenuPanel.SetActive(false);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        exitButton.onClick.AddListener(ExitGame);

        if (PlayerPrefs.HasKey("volume"))
        {
            float volume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = volume;
            SetVolume(volume);
        }
    }

    public void ToggleOptionsMenu()
    {
        optionsMenuPanel.SetActive(!optionsMenuPanel.activeSelf);
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("coins", GameObject.Find("PlayerManager").GetComponent<PlayerManager>().coins);
        PlayerPrefs.SetInt("health", GameObject.Find("PlayerManager").GetComponent<PlayerManager>().health);
        PlayerPrefs.Save();
        Debug.Log("Game saved.");
    }

    private void LoadGame()
    {
        var player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        player.coins = PlayerPrefs.GetInt("coins", 100);
        player.health = PlayerPrefs.GetInt("health", 50);
        Debug.Log("Game loaded.");
    }

    private void ExitGame()
    {
        Debug.Log("Quitting game...");
        SceneManager.LoadScene("Scenes/Menu/MainMenu");

        SaveGame();

    }
}
