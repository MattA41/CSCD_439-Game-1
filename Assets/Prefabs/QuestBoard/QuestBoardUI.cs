using System.Collections.Generic;
using UnityEngine;

public class QuestBoardUI : MonoBehaviour
{
    public GameObject questBoardPanel;
    public GameObject questEntryPrefab;
    public Transform questContentArea;
    public PlayerManager playerManager;
    public EnemySpawner enemySpawner;
    private bool hasCompletedRound5Quest = false;


    private bool hasSpawned = false;


    // Track quests by title
    private Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>();

    void Start()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        AddQuest("Make it to Round 5 without damage", "Get 100 coins if you make it to round 5 without losing any coins!");
        AddQuest("Seasoned Vet", "Get to round 20 without losing any health.");
    }

    void Update()
    {
        if (!hasCompletedRound5Quest && enemySpawner.currWave == 5 && playerManager.health == 50) //Add coins if met
        {
            EditQuest("Make it to Round 5 without damage", "Complete! 100 Coins added.");
            playerManager.coins = playerManager.coins + 100;
            hasCompletedRound5Quest = true;
        }
        else if(enemySpawner.currWave > 5 && playerManager.health <= 50)  //Delete Quest if not met
        {
            RemoveQuest("Make it to Round 5 without damage");
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
