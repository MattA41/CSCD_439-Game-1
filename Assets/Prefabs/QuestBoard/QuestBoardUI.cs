using UnityEngine;

public class QuestBoardUI : MonoBehaviour
{
    public GameObject questBoardPanel;
    public GameObject questEntryPrefab;
    public Transform questContentArea;
    public PlayerManager playerManager;

    private bool hasSpawned = false;
    private bool hasTriggeredHighCoinsQuest = false;

    void Start()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Seasoned Vet", "Get to round 20 without losing any health.");
    }

    void Update()
    {
        if (!hasTriggeredHighCoinsQuest && playerManager.coins >= 500)  //Trigger if we want to add quests mid game use this if statement
        {
            AddQuest("NiceJob", "You got more than 500 coins!");
            hasTriggeredHighCoinsQuest = true;
        }
    }

    public void AddQuest(string title, string description)
    {
        GameObject questGO = Instantiate(questEntryPrefab, questContentArea);
        QuestEntry entry = questGO.GetComponent<QuestEntry>();
        entry.SetQuest(title, description);
    }
}
