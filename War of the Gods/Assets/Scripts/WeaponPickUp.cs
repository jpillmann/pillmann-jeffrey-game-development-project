using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        // Pick Up WeaponItem, add it to the Players Inventory and destroy the GameObject in the WorldSpace
        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerMovement playerMovement;
            // LATER: AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerMovement = playerManager.GetComponent<PlayerMovement>();
            // LATER: animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            // stops the Player from moving while picking up item
            playerMovement.rigidbody.velocity = Vector3.zero;
            // LATER: animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
