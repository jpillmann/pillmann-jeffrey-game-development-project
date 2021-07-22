using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCMovementManager : MonoBehaviour
    {
        NPCManager npcManager;
        NPCAnimatorHandler npcAnimatorHandler;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterColliderBlocker;

        public LayerMask detectionLayer;

        private void Awake()
        {
            npcManager = GetComponent<NPCManager>();
            npcAnimatorHandler = GetComponentInChildren<NPCAnimatorHandler>();
        }

        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterColliderBlocker, true);
        }
    }
}
