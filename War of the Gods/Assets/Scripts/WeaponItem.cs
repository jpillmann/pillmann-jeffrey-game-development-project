using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP {
    [CreateAssetMenu(menuName = "Items/Weapon Item")]

    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        public WeaponType weaponType;

        [Header("Physical Damage Range")]
        public int minDamage;
        public int maxDamage;

        [Header("Magical Damage Range")]
        public int minMagicDamage;
        public int maxMagicDamage;

        [Header("Blood Damage Range")]
        public int minBloodDamage;
        public int maxBloodDamage;

        public bool isMagic;
        public bool isCursed;

        public int damage;
        public int magicDamage;
        public int bloodDamage;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_01;
        public string OH_Light_Attack_02;
        public string OH_Heavy_Attack_01;
        public string OH_Heavy_Attack_02;
    }

    public enum WeaponType
    {
        Sword,
        Axe,
        Mace,
        Greatsword,
        Greataxe,
        Warhammer,
        Shield,
        Staff
    }
}
