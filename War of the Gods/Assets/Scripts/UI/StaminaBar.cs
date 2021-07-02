using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        // Set Stamina Bar Slider to Max Value
        public void SetMaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        // Set Stamina Bar Slider to current stamina value
        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }
    }
}
