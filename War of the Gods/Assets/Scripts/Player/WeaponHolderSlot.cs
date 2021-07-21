using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;
        public WeaponItem currentWeapon;

        // Unload current Weapon Model
        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        // Destroy current Weapon Model
        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        // Load WeaponModel
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
}
