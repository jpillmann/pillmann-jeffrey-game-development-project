using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    // UI Slot for Displaying Quests
    public class QuestSlot : MonoBehaviour
    {
        UIManager uiManager;

        Quest quest;
        public Text questDescription;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        // Add a Quest to UI Slot
        public void AddQuest(Quest newQuest)
        {
            quest = newQuest;
            questDescription.text = quest.description;
            questDescription.enabled = true;
            gameObject.SetActive(true);
        }

        // Remove a Quest from UI
        public void ClearQuestSlot()
        {
            quest = null;
            questDescription.text = null;
            questDescription.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
