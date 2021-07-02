using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP {
    public class DialogueTween : MonoBehaviour
    {
        public GameObject interactableUIDialogueObject;

        public void ShowDialogue()
        {
            transform.LeanMoveLocal(new Vector2(0, 21), 1).setEaseOutQuart();
        }

        public void HideDialogue()
        {
            transform.LeanMoveLocal(new Vector2(0, -190), 1).setEaseOutQuart();
            StartCoroutine(HandleDialogueWindow());
        }

        private IEnumerator HandleDialogueWindow()
        {
            yield return new WaitForSeconds(2);
            interactableUIDialogueObject.SetActive(false);
        }
    }
}
