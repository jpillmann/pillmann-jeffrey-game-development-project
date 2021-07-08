using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class FleeingState : State
    {
        public IdleState idleState;
        public LayerMask detectionLayer;
        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            Vector3 targetDirection = npcManager.currentTarget.transform.position - npcManager.transform.position;
            float distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, npcManager.transform.forward);

            Vector3 fleeingDirection = -targetDirection;
            fleeingDirection.Normalize();
            Vector3 fleeingPosition = npcManager.transform.position + (fleeingDirection * 5);

            if (distanceFromTarget < npcManager.minimumDesiredDistance)
            {
                npcAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateAwayFromTarget(npcManager, fleeingPosition);

            npcManager.navMeshAgent.transform.localPosition = Vector3.zero;
            npcManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget >= npcManager.minimumDesiredDistance)
            {
                npcManager.currentTarget = null;
                return idleState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateAwayFromTarget(NPCManager npcManager, Vector3 fleeingPosition)
        {
            Vector3 relativeDirection = npcManager.transform.InverseTransformDirection(npcManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = npcManager.npcRigidbody.velocity;

            npcManager.navMeshAgent.enabled = true;
            npcManager.navMeshAgent.SetDestination(fleeingPosition);
            npcManager.npcRigidbody.velocity = targetVelocity;
            npcManager.transform.rotation = Quaternion.Slerp(npcManager.transform.rotation, npcManager.navMeshAgent.transform.rotation, npcManager.rotationSpeed / Time.deltaTime);
        }
    }
}