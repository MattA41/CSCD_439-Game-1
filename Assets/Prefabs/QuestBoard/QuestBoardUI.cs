using UnityEngine;

public class QuestBoardUI : MonoBehaviour
{
    public GameObject questBoardPanel;
    public GameObject questEntryPrefab;
    public Transform questContentArea;
    //private bool isOpen = false;
    private bool hasSpawned = false;



    void Start()
    {
        if (hasSpawned) return;
        hasSpawned = true;
        // Test quest to make sure it's working
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Rescue the Villager", "Locate and escort the missing villager back.");
        
    }



    //public void ToggleQuestBoard()
    //{
    //    isOpen = !isOpen;
    //    questBoardPanel.SetActive(isOpen);
    //}

    public void AddQuest(string title, string description)

    {

        GameObject questGO = Instantiate(questEntryPrefab, questContentArea);
        QuestEntry entry = questGO.GetComponent<QuestEntry>();
        entry.SetQuest(title, description);
    }

  
}
