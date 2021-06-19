using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class QuestSlot : MonoBehaviour
    {
        UIManager uiManager;

        Quest quest;
        public Text questDescription;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddQuest(Quest newQuest)
        {
            quest = newQuest;
            questDescription.text = quest.description;
            questDescription.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearQuestSlot()
        {
            quest = null;
            questDescription.text = null;
            questDescription.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
