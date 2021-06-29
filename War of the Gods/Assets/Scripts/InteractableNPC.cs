using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class InteractableNPC : Interactable
    {
        public string interactabeNPCName;
        public string interactableNPCDialogue;

        public bool hasQuest;
        public Quest quest;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            StartDialogue(playerManager);
        }

        // Sets UI Dialogue and it's children active and sets texts on interaction with player
        // If NPC has a quest to give set
        private void StartDialogue(PlayerManager playerManager)
        {
            PlayerMovement playerMovement;
            playerMovement = playerManager.GetComponent<PlayerMovement>();
            playerMovement.rigidbody.velocity = Vector3.zero; // stops the Player from moving while interacting with npc's

            #region Dialogue

            playerManager.interactableNPCName.GetComponentInChildren<Text>().text = interactabeNPCName;
            playerManager.interactableNPCDialogue.GetComponentInChildren<Text>().text = interactableNPCDialogue;
            playerManager.interactableUIDialogueObject.SetActive(true);
            playerManager.interactableUIDialogueObject.GetComponent<DialogueTween>().ShowDialogue();
            #endregion

            #region Quest

            // Set hasQuest false if the NPCs Quest has already been completed
            for (int i = 0; i < playerManager.completedQuests.Count; i++)
            {
                if (playerManager.completedQuests[i].title == quest.title)
                {
                    quest.isActive = false;
                    hasQuest = false;
                    break;
                }
            }

            // Set UI Quest Object if NPC has Quest that has not been accepted or completed yet
            if (hasQuest)
            {
                if (quest.isActive)
                {
                    playerManager.completeQuestTitle.GetComponentInChildren<Text>().text = quest.title;
                    playerManager.tempQuest = quest;
                    playerManager.interactableUICompleteQuestObject.SetActive(true);
                    playerManager.interactableUICompleteQuestObject.GetComponent<QuestTween>().Open();
                }
                else
                {
                    playerManager.questTitle.GetComponentInChildren<Text>().text = quest.title;
                    playerManager.questDescription.GetComponentInChildren<Text>().text = quest.description;
                    playerManager.tempQuest = quest;
                    playerManager.interactableUIQuestObject.SetActive(true);
                    playerManager.interactableUIQuestObject.GetComponent<QuestTween>().Open();
                }
            }
            #endregion
        }
    }
}
