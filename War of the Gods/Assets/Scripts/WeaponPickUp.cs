using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Pick Up WeaponItem and add it to the Players Inventory
        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerMovement playerMovement;
            //AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerMovement = playerManager.GetComponent<PlayerMovement>();
            //animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            // stops the play from moving while picking up item
            playerMovement.rigidbody.velocity = Vector3.zero;
            //animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.weaponsInventory.Add(weapon);
            Destroy(gameObject);
        }
    }
}
