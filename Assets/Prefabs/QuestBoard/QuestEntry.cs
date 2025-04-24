using UnityEngine;
using UnityEngine.UI;

public class QuestEntry : MonoBehaviour
{
    public Text titleText;
    public Text descriptionText;

    public void SetQuest(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
 
}