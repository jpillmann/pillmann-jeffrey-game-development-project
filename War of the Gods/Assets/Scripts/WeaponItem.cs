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
        public string OH_Light_Attack_01;
        public string OH_Light_Attack_02;
        public string OH_Light_Attack_03;
        public string OH_Heavy_Attack_01;
        public string OH_Sneak_Attack_01;
    }
}
