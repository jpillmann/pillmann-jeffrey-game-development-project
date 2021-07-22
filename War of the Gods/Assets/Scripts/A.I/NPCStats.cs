using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCStats : CharacterStats
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

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
            if (isDead)
                return;

            currentHealth -= damage;
            animator.Play("Take-Damage-01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death-04");
                isDead = true;
            }
        }
    }
}
