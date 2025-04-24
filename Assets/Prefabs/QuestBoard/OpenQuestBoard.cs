using UnityEngine;

public class OpenQuestBoard : MonoBehaviour
{
    public GameObject questBoardPanel;
 
    private bool isOpen = false;
    



   


    public void ToggleQuestBoard()
    {
        isOpen = !isOpen;
        questBoardPanel.SetActive(isOpen);
    }

  
}
