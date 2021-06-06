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

        public int sprintStaminaCost = 10;
        public int rollStaminaCost = 5;

        public HealthBar healthBar;
        public StaminaBar staminaBar;

        private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
        private Coroutine regen;

        // Start is called before the first frame update
        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaRegen = SetStaminaRegenFromStaminaLevel();
        }

        #region Health
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);

            // Play "take damage" animation

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                // Handle Player Death
            }
        }
        #endregion

        #region Stamina

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        private int SetStaminaRegenFromStaminaLevel()
        {
            staminaRegen = Mathf.RoundToInt(staminaLevel / 2);
            return staminaRegen;
        }

        public void UseStamina(int staminaCost)
        {
            if (currentStamina - staminaCost >= 0)
            {
                currentStamina = currentStamina - staminaCost;

                staminaBar.SetCurrentStamina(currentStamina);

                if (regen != null)
                {
                    StopCoroutine(regen);
                }

                regen = StartCoroutine(RegenerateStamina());
            }
            else
            {
                regen = StartCoroutine(RegenerateStamina());
            }
        }

        public IEnumerator RegenerateStamina()
        {
            yield return new WaitForSeconds(2);

            while (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen;
                staminaBar.SetCurrentStamina(currentStamina);
                yield return regenTick;
            }
            regen = null;
        }
        #endregion
    }
}
