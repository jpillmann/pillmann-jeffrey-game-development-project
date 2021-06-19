using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        // Visual Help
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        // Called When Player Interacts with Interactable
        // This function will be overriden by Interactable Objects or other Classes Inhereting from Interactable
        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("You interacted with an Object!");
        }
    }
}
