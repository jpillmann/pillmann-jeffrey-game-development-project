using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    [System.Serializable]
    public class Bonus
    {
        public string deity;

        public BonusType bonusType;
        public float noviceBonus;
        public float priestBonus;
        public WeaponItem championWeapon;

        [Header("Descriptions")]
        public string noviceBonusTitle;
        public string noviceBonusDescription;
        public string priestBonusTitle;
        public string priestBonusDescription;
        public string championBonusTitle;
        public string championBonusDescription;
    }

    public enum BonusType
    {
        Weapon,
        Staff,
        Magic,
        Blood,
        Dual,
        Armor
    }
}
