using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    // Interactable UI values are set and displayed according to what the Player interacts with
    public class InteractableUI : MonoBehaviour
    {
        // Pick Up Items
        public Text interactableText;
        public Text itemText;
        public RawImage itemIcon;

        // Dialogue
        public Text interactableNPCName;
        public Text interactableNPCDialogue;
    }
}
