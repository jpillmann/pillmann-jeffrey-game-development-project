using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    [System.Serializable]
    public class QuestGoal
    {
        public GoalType goalType;

        public Item item;
        public int requiredAmount;
        public int currentAmount;

        // Returns True if the QuestGoal is reached
        public bool IsReached()
        {
            return (currentAmount >= requiredAmount);
        }

        // Checks the Players Inventory for the required Item / Items
        // modular, will be expanded, when further inventory sections are added
        public void CheckCurrentAmount(PlayerInventory playerInventory)
        {
            if (item is WeaponItem)
            {
                for (int i = 0; i < playerInventory.weaponsInventory.Count; i++)
                {
                    if (playerInventory.weaponsInventory[i] == item)
                    {
                        currentAmount++;
                    }
                }
            }
        }
    }

    // Types of Quest are Listed here
    public enum GoalType
    {
        Gathering
    }
}
