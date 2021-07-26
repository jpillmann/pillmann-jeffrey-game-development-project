using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        public PlayerInventory playerInventory;
        public float currentWeaponDamage;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private float calcDamageOnPlayer(WeaponItem weapon, NPCStats npcStats)
        {
            float damage = weapon.damage;
            float magicDamage = 0f;
            float bloodDamage = 0f;

            if (weapon.weaponType == WeaponType.Sword)
            {
                damage *= npcStats.swordDamageMultiplier;
            }
            else if (weapon.weaponType == WeaponType.Axe)
            {
                damage *= npcStats.axeDamageMultiplier;
            }
            else if (weapon.weaponType == WeaponType.Mace)
            {
                damage *= npcStats.maceDamageMultiplier;
            }
            else if (weapon.weaponType == WeaponType.Staff)
            {
                damage *= npcStats.staffDamageMultiplier;
            }

            if (weapon.isMagic)
            {
                magicDamage = weapon.magicDamage;
                magicDamage *= npcStats.magicDamageMultiplier;
                damage += magicDamage;
            }
            else if (weapon.isCursed)
            {
                bloodDamage = weapon.bloodDamage;
                bloodDamage *= npcStats.bloodDamageMultiplier;
                damage += bloodDamage;
            }

            return damage;
        }

        private float calcDamageOnNPC(WeaponItem weapon, PlayerStats playerStats)
        {
            float damage = weapon.damage;
            float magicDamage = 0f;
            float bloodDamage = 0f;

            if (weapon.weaponType == WeaponType.Sword)
            {
                damage *= playerStats.swordDamageMultiplier;
            }
            else if (weapon.weaponType == WeaponType.Axe)
            {
                damage *= playerStats.axeDamageMultiplier;
            }
            else if (weapon.weaponType == WeaponType.Mace)
            {
                damage *= playerStats.maceDamageMultiplier;
            }
            else if (weapon.weaponType == WeaponType.Staff)
            {
                damage *= playerStats.staffDamageMultiplier;
            }

            if (weapon.isMagic)
            {
                magicDamage = weapon.magicDamage;
                magicDamage *= playerStats.magicDamageMultiplier;
                damage += magicDamage;
            }
            else if (weapon.isCursed)
            {
                bloodDamage = weapon.bloodDamage;
                bloodDamage *= playerStats.bloodDamageMultiplier;
                damage += bloodDamage;
            }

            return damage;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                PlayerManager playerManager = FindObjectOfType<PlayerManager>();
                NPCStats npcStats = GetComponentInParent<NPCStats>();
                WeaponItem weapon = npcStats.weapon;

                if (playerStats != null)
                {
                    if (playerManager.isBlocking)
                    {
                        playerStats.armorMultiplier = playerStats.armorMultiplier - (playerInventory.leftWeapon.blockingValue / 100);
                    }
                    else
                    {
                        playerStats.armorMultiplier = 1.0f;
                    }

                    currentWeaponDamage = calcDamageOnPlayer(weapon, npcStats);
                    currentWeaponDamage *= playerStats.armorMultiplier;
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if (collision.tag == "Enemy")
            {
                NPCStats npcStats = collision.GetComponent<NPCStats>();
                PlayerStats playerStats = FindObjectOfType<PlayerStats>();
                PlayerManager playerManager = FindObjectOfType<PlayerManager>();

                if (npcStats != null)
                {
                    if (playerManager.isUsingRightHand)
                    {
                        currentWeaponDamage = calcDamageOnNPC(playerInventory.rightWeapon, playerStats);
                    }
                    else if (playerManager.isUsingLeftHand)
                    {
                        currentWeaponDamage = calcDamageOnNPC(playerInventory.leftWeapon, playerStats);
                    }

                    if (playerInventory.rightWeapon.weaponType != WeaponType.Unarmed 
                        && playerInventory.rightWeapon.weaponType != WeaponType.Shield 
                        && playerInventory.rightWeapon.weaponType != WeaponType.Staff 
                        && playerInventory.leftWeapon.weaponType != WeaponType.Unarmed 
                        && playerInventory.leftWeapon.weaponType != WeaponType.Shield 
                        && playerInventory.leftWeapon.weaponType != WeaponType.Staff)
                    {
                        currentWeaponDamage *= playerStats.dualWieldMultiplier;
                    }

                    currentWeaponDamage *= playerStats.armorMultiplier;
                    npcStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}
