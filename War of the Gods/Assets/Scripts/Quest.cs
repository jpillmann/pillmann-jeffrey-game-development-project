using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    [System.Serializable]
    public class Quest
    {
        // Quest Values and Goals are Set in Inspector
        public bool isActive;

        public string title;
        public string description;
        public WeaponItem weaponReward;
        public QuestType questType;

        public QuestGoal questGoal;
    }

    public enum QuestType
    {
        SideQuest,
        MainQuest
    }
}
