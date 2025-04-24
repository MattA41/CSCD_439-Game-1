using System.Collections.Generic;
using UnityEngine;

public class QuestBoardUI : MonoBehaviour
{
    public GameObject questBoardPanel;
    public GameObject questEntryPrefab;
    public Transform questContentArea;
    public PlayerManager playerManager;
    public EnemySpawner enemySpawner;

    private bool hasSpawned = false;
    private bool hasTriggeredHighCoinsQuest = false;

    // Track quests by title
    private Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>();

    void Start()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        AddQuest("Defeat 3 Goblins", "Find and eliminate 3 goblins near the camp.");
        AddQuest("Seasoned Vet", "Get to round 20 without losing any health.");
    }

    void Update()
    {
        if (!hasTriggeredHighCoinsQuest && playerManager.coins >= 500)
        {
            AddQuest("NiceJob", "You got more than 500 coins!");
            hasTriggeredHighCoinsQuest = true;
        }
        if (!hasTriggeredHighCoinsQuest && enemySpawner.currWave >= 2)
        {
            EditQuest("Defeat 3 Goblins", "LOL");
        }

    }

    public void AddQuest(string title, string description)
    {
        if (activeQuests.ContainsKey(title))
        {
            Debug.LogWarning($"Quest '{title}' already exists.");
            return;
        }

        GameObject questGO = Instantiate(questEntryPrefab, questContentArea);
        QuestEntry entry = questGO.GetComponent<QuestEntry>();
        entry.SetQuest(title, description);

        activeQuests[title] = questGO;
    }

    public void RemoveQuest(string title)
    {
        if (activeQuests.TryGetValue(title, out GameObject questGO))
        {
            Destroy(questGO);
            activeQuests.Remove(title);
        }
        else
        {
            Debug.LogWarning($"Quest '{title}' not found.");
        }
    }

    public void EditQuest(string title, string newDescription)
    {
        if (activeQuests.TryGetValue(title, out GameObject questGO))
        {
            QuestEntry entry = questGO.GetComponent<QuestEntry>();
            entry.SetQuest(title, newDescription); // assumes SetQuest sets both title and description
        }
        else
        {
            Debug.LogWarning($"Quest '{title}' not found for editing.");
        }
    }
}
