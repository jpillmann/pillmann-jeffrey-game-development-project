using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP {
    [CreateAssetMenu(menuName = "Items/Weapon Item")]

    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")]
        public int minDamage;
        public int maxDamage;

        [Header("Magic Damage")]
        public int minMagicDamage;
        public int maxMagicDamage;

        [Header("Blood Damage")]
        public int minBloodDamage;
        public int maxBloodDamage;

        [HideInInspector]
        public bool isMagic;
        [HideInInspector]
        public bool isCursed;
        [HideInInspector]
        public int damage;
        [HideInInspector]
        public int magicDamage;
        [HideInInspector]
        public int bloodDamage;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_01;
        public string OH_Light_Attack_02;
        public string OH_Light_Attack_03;
        public string OH_Heavy_Attack_01;
        public string OH_Sneak_Attack_01;
    }
}
