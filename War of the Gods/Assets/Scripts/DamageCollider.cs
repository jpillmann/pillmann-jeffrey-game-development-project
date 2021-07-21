using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        public PlayerInventory playerInventory;
        public int currentWeaponDamage;

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

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    currentWeaponDamage = 25;
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if (collision.tag == "Enemy")
            {
                NPCStats npcStats = collision.GetComponent<NPCStats>();

                if (npcStats != null)
                {
                    currentWeaponDamage = playerInventory.rightWeapon.damage;
                    npcStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}
