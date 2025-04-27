using UnityEngine;

public class PreloadResources : MonoBehaviour
{
    private void Awake()
    {
        var click = Resources.Load<AudioClip>("Sounds/ButtonClick");

        if (click == null)
            Debug.LogError("PreloadResources: ButtonClick.wav not found!");
        else
            Debug.Log("PreloadResources: ButtonClick.wav loaded successfully.");
    }
}