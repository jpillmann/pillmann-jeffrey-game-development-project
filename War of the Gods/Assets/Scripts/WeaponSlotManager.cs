using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class WeaponSlotManager : MonoBehaviour
    {
        PlayerManager playerManager;
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        QuickSlotsUI quickSlotsUI;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        // Load Equipped Weapon Models
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftHandDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightHandDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }

        #region Handle Damage Colliders

        private void LoadLeftHandDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightHandDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }

        #endregion
    }
}
