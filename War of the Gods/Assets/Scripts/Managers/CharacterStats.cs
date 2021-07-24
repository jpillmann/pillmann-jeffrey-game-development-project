using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class CharacterStats : MonoBehaviour
    {
        public float healthLevel = 10;
        public float maxHealth;
        public float currentHealth;

        public float staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;
        public float staminaRegen;
        public float staminaRegenTimer = 0;

        public bool isDead;

        [Header("Multipliers")]
        public float swordDamageMultiplier = 1.0f;
        public float axeDamageMultiplier = 1.0f;
        public float maceDamageMultiplier = 1.0f;
        public float greatswordDamageMultiplier = 1.0f;
        public float greataxeDamageMultiplier = 1.0f;
        public float warhammerDamageMultiplier = 1.0f;
        public float bloodDamageMultiplier = 1.0f;
        public float magicDamageMultiplier = 1.0f;
        public float armorMultiplier = 1.0f;
    }
}
