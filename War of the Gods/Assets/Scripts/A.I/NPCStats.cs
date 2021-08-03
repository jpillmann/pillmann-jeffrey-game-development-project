using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCStats : CharacterStats
    {
        Animator animator;
        NPCManager npcManager;
        NPCWeaponSlotManager npcWeaponSlotManager;
        PlayerStats playerStats;
        public WeaponItem weapon;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            npcManager = GetComponent<NPCManager>();
            npcWeaponSlotManager = GetComponentInChildren<NPCWeaponSlotManager>();
            playerStats = FindObjectOfType<PlayerStats>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            weapon = npcWeaponSlotManager.rightHandWeapon;
            faction = npcManager.faction;
        }

        // Set MaxHealth value from NPC's Health Level
        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        // NPC takes damage to Health stat according to damage value
        public void TakeDamage(float damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;
            animator.Play("Take-Damage-01");

            if (currentHealth <= 0)
            {
                if (faction != playerStats.faction)
                {
                    playerStats.enemiesKilled++;
                    playerStats.enemiesKilledForQuest++;
                }
                else if (faction == playerStats.faction)
                {
                    playerStats.friendsKilled++;
                }

                currentHealth = 0;
                animator.Play("Death-04");
                isDead = true;
                npcManager.currentState = null;
            }
        }
    }
}
