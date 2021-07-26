using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        AnimatorHandler animatorHandler;
        public HealthBar healthBar;
        public StaminaBar staminaBar;

        public Bonus bonus;
        public string worshipTitle;
        public int favor = 0;
        public int enemiesKilled = 0;
        public int friendsKilled = 0;
        public int mainQuestsCompleted = 0;
        public int sideQuestsCompleted = 0;


        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerInventory = GetComponent<PlayerInventory>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
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
        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        // Player takes damage to Health stat according to damage value
        public void TakeDamage(float damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Take-Damage-01", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death-04", true);
                isDead = true;
            }
        }
        #endregion

        #region Stamina

        // Set the Max Stamina based on the Players StaminaLevel
        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        // Depelte Stamina based on Stamina Cost
        public void TakeStaminaDamage(float staminaCost)
        {
            currentStamina -= staminaCost;

            staminaBar.SetCurrentStamina(currentStamina);
        }

        // Regenerate Stamina on 1s delay if the player is currently not performing an action (i.e rolling, sprinting)
        public void RegenerateStamina()
        {
            if (playerManager.isInteracting || playerManager.isSprinting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegen * Time.deltaTime;
                    staminaBar.SetCurrentStamina(currentStamina);
                }
            }
        }
        #endregion

        #region Favor

        public void HandleFavor()
        {
            if (bonus != null)
            {
                favor += enemiesKilled * 2;
                favor += sideQuestsCompleted * 5;
                favor += mainQuestsCompleted * 10;
                favor -= friendsKilled * 25;

                enemiesKilled = 0;
                friendsKilled = 0;
                sideQuestsCompleted = 0;
                mainQuestsCompleted = 0;

                // Change the Worship Title / Boni of the character
                if (favor >= 100 && favor < 150 && worshipTitle == "Novice")
                {
                    worshipTitle = "Priest";

                    if (bonus.bonusType == BonusType.Weapon)
                    {
                        swordDamageMultiplier = bonus.priestBonus;
                        axeDamageMultiplier = bonus.priestBonus;
                        maceDamageMultiplier = bonus.priestBonus;
                    }
                    else if (bonus.bonusType == BonusType.Staff)
                    {
                        staffDamageMultiplier = bonus.priestBonus;
                    }
                    else if (bonus.bonusType == BonusType.Dual)
                    {
                        dualWieldMultiplier = bonus.priestBonus;
                    }
                    else if (bonus.bonusType == BonusType.Magic)
                    {
                        magicDamageMultiplier = bonus.priestBonus;
                    }
                    else if (bonus.bonusType == BonusType.Blood)
                    {
                        bloodDamageMultiplier = bonus.priestBonus;
                    }
                    else if (bonus.bonusType == BonusType.Armor)
                    {
                        armorMultiplier = bonus.priestBonus;
                    }
                }
                else if (favor >= 150 && worshipTitle == "Priest")
                {
                    worshipTitle = "Champion";

                    playerInventory.weaponsInventory.Add(bonus.championWeapon);
                }
                else if (favor < 25)
                {
                    worshipTitle = null;
                    bonus = null;
                }

                Debug.Log(favor);
            }
        }

        #endregion
    }
}
