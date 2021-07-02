using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCStats : CharacterStats
    {
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
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
            currentHealth -= damage;
            // TODO: Play "take damage" animation

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                // TODO: Handle NPC Death
            }
        }
    }
}
