using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCAnimatorHandler : AnimatorManager
    {
        NPCMovementManager npcMovementManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            npcMovementManager = GetComponentInParent<NPCMovementManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            npcMovementManager.npcRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            npcMovementManager.npcRigidbody.velocity = velocity;
        }
    }
}
