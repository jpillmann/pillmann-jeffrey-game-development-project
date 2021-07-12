using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public WeaponItem generatedWeapon;

        private void Start()
        {
            GenerateWeapon();
        }

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

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerMovement = playerManager.GetComponent<PlayerMovement>();

            // stops the Player from moving while picking up item
            playerMovement.rigidbody.velocity = Vector3.zero;

            playerInventory.weaponsInventory.Add(generatedWeapon);
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<Text>().text = generatedWeapon.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = generatedWeapon.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);
        }

        // Procedurally generate weapon based on item prefab
        private void GenerateWeapon()
        {
            generatedWeapon = ScriptableObject.CreateInstance("WeaponItem") as WeaponItem;
            generatedWeapon.modelPrefab = weapon.modelPrefab;
            generatedWeapon.itemIcon = weapon.itemIcon;

            // Generate Damage Value
            generatedWeapon.damage = Random.Range(weapon.minDamage, weapon.maxDamage);

            // Genetate Magic / Cursed Trait
            float value = Random.value;
            if (value < 0.25f)
            {
                generatedWeapon.isMagic = true;
                generatedWeapon.isCursed = false;
            }
            else if (value >= 0.25f && value < 0.5f)
            {
                generatedWeapon.isMagic = false;
                generatedWeapon.isCursed = true;
            }
            else
            {
                generatedWeapon.isMagic = false;
                generatedWeapon.isCursed = false;
            }

            // Generate Trait Damage & Set Itemname
            if (generatedWeapon.isMagic)
            {
                generatedWeapon.magicDamage = Random.Range(weapon.minMagicDamage, weapon.maxMagicDamage);
                generatedWeapon.itemName = "Magic " + weapon.itemName;
            }
            else if (generatedWeapon.isCursed)
            {
                generatedWeapon.bloodDamage = Random.Range(weapon.minBloodDamage, weapon.maxBloodDamage);
                generatedWeapon.itemName = "Cursed " + weapon.itemName;
            }
            else
            {
                generatedWeapon.itemName = weapon.itemName;
            }
        }
    }
}
