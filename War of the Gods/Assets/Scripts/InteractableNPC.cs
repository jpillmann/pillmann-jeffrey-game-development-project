using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class InteractableNPC : Interactable
    {
        public string interactabeNPCName;
        public string interactableNPCDialogue = "Hello There!";

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            StartDialogue(playerManager);
        }

        private void StartDialogue(PlayerManager playerManager)
        {
            PlayerMovement playerMovement;
            playerMovement = playerManager.GetComponent<PlayerMovement>();

            // stops the Player from moving while interacting with npc's
            playerMovement.rigidbody.velocity = Vector3.zero;
            playerManager.interactableNPCName.GetComponentInChildren<Text>().text = interactabeNPCName;
            playerManager.interactableNPCDialogue.GetComponentInChildren<Text>().text = interactableNPCDialogue;
            playerManager.interactableUIDialogueObject.SetActive(true);
        }
    }
}
