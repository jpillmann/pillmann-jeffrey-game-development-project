using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        PlayerStats playerStats;
        public string lastAttack;

        [Header("Stamina Costs")]
        public float lightAttackStaminaCost = 15;
        public float heavyAttackStaminaCost = 30;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponent<PlayerStats>();
        }

        public void HandleRightLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Right_Light_Attack_01, true);
            lastAttack = weapon.OH_Right_Light_Attack_01;
            playerStats.TakeStaminaDamage(lightAttackStaminaCost);
        }

        public void HandleRightHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Right_Heavy_Attack_01, true);
            lastAttack = weapon.OH_Right_Heavy_Attack_01;
            playerStats.TakeStaminaDamage(heavyAttackStaminaCost);
        }

        public void HandleRightLightAttackCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Right_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Right_Light_Attack_02, true);
                    playerStats.TakeStaminaDamage(lightAttackStaminaCost);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Right_Light_Attack_01, true);
                    playerStats.TakeStaminaDamage(lightAttackStaminaCost);
                }
            }
        }

        public void HandleRightHeavyAttackCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Right_Heavy_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Right_Heavy_Attack_02, true);
                    playerStats.TakeStaminaDamage(heavyAttackStaminaCost);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Right_Heavy_Attack_01, true);
                    playerStats.TakeStaminaDamage(heavyAttackStaminaCost);
                }
            }
        }

        public void HandleLeftLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Left_Light_Attack_01, true);
            lastAttack = weapon.OH_Left_Light_Attack_01;
            playerStats.TakeStaminaDamage(lightAttackStaminaCost);
        }

        public void HandleLeftLightAttackCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Left_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Left_Light_Attack_02, true);
                    playerStats.TakeStaminaDamage(lightAttackStaminaCost);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Left_Light_Attack_01, true);
                    playerStats.TakeStaminaDamage(lightAttackStaminaCost);
                }
            }
        }

        public void HandleBlocking()
        {
            animatorHandler.PlayTargetAnimation("Block-Start", false);
        }
    }
}
