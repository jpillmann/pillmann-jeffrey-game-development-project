using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP {
    [CreateAssetMenu(menuName = "Items/Weapon Item")]

    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Heavey_Attack_1;
    }
}
