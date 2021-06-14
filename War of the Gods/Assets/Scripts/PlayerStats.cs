using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public int maxStamina;
        public int currentStamina;
        public int staminaRegen;
        public float staminaRegenTimer = 0;

        PlayerManager playerManager;
        public HealthBar healthBar;
        public StaminaBar staminaBar;


        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        // Set Values for Players Max and Current Health, Stamina
        // Set Health and Stamina Bar UI Sliders
        // Init StaminaRegen value
        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaRegen = 40;
        }

        #region Health

        // Set MaxHealth value from Players Health Level
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        // Player takes damage to Health stat according to damage value
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            // TODO: Play "take damage" animation

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                // TODO: Handle Player Death
            }
        }
        #endregion

        #region Stamina

        // Set the Max Stamina based on the Players StaminaLevel
        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        // Depelte Stamina based on Stamina Cost
        public void TakeStaminaDamage(int staminaCost)
        {
            currentStamina -= staminaCost;

            staminaBar.SetCurrentStamina(currentStamina);
        }

        // Regenerate Stamina on 1s delay if the player is currently not performing an action (i.e rolling, sprinting)
        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += Mathf.RoundToInt(staminaRegen * Time.deltaTime);
                    staminaBar.SetCurrentStamina(currentStamina);
                }
            }
        }
        #endregion
    }
}
