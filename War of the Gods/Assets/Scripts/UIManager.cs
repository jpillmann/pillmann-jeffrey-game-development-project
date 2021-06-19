using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class UIManager : MonoBehaviour
    {
        public PlayerManager playerManager;
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;
        public GameObject questListWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;


        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        [Header("Quests List")]
        public GameObject questSlotPrefab;
        public Transform questsList;
        QuestSlot[] questSlots;

        [Header("Completed Quests List")]
        public GameObject completedQuestSlotPrefab;
        public Transform completedQuestsList;
        QuestSlot[] completedQuestSlots;


        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);

            questSlots = questsList.GetComponentsInChildren<QuestSlot>();
            completedQuestSlots = completedQuestsList.GetComponentsInChildren<QuestSlot>();
        }

        // Updates Weapon Inventory and Quest List UI
        // Add/ Remove Slots according to List Size (WeaponsInventory, QuestsList, CompletedQuestsList)
        // Add Items/ Quests onto Slots in UI
        public void UpdateUI()
        {
            #region Weapon Inventory
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Quest Lists
            // Quests List
            for (int i = 0; i < questSlots.Length; i++)
            {
                if (i < playerManager.quests.Count)
                {
                    if (questSlots.Length < playerManager.quests.Count)
                    {
                        Instantiate(questSlotPrefab, questsList);
                        questSlots = questsList.GetComponentsInChildren<QuestSlot>();
                    }
                    questSlots[i].AddQuest(playerManager.quests[i]);
                }
                else
                {
                    questSlots[i].ClearQuestSlot();
                }
            }

            // Completed Quests List
            for (int i = 0; i < completedQuestSlots.Length; i++)
            {
                if (i < playerManager.completedQuests.Count)
                {
                    if (completedQuestSlots.Length < playerManager.completedQuests.Count)
                    {
                        Instantiate(completedQuestSlotPrefab, completedQuestsList);
                        completedQuestSlots = completedQuestsList.GetComponentsInChildren<QuestSlot>();
                    }
                    completedQuestSlots[i].AddQuest(playerManager.completedQuests[i]);
                }
                else
                {
                    completedQuestSlots[i].ClearQuestSlot();
                }
            }
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            questListWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }
}
