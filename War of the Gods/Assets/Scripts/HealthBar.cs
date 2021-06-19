using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        // Set Health Bar Slider to Max Value
        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        // Set Health Bar to current Health value
        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}
