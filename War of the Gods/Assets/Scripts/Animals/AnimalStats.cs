using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class AnimalStats : CharacterStats
    {
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        // Set MaxHealth value from Animals Health Level
        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        // Animal takes damage to Health stat according to damage value
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            // TODO: Play animal damage sound

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                // TODO: Handle Animal Death
            }
        }
    }
}
