using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCAnimatorHandler : AnimatorManager
    {
        NPCManager npcManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            npcManager = GetComponentInParent<NPCManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            npcManager.npcRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            npcManager.npcRigidbody.velocity = velocity;
        }
    }
}
