using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    // Item Class
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;
    }
}
