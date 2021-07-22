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
    }
}
