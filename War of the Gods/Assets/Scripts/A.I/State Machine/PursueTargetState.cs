using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            // Chase Target
            // If within attack range, switch to combat stance state
            // if target is out of range, return this state and continue pursueing target
            if (npcManager.isPerformingAction)
            {
                npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            Vector3 targetDirection = npcManager.currentTarget.transform.position - npcManager.transform.position;
            float distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, npcManager.transform.forward);

            if (distanceFromTarget > npcManager.maximumAttackRange)
            {
                npcAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(npcManager);

            npcManager.navMeshAgent.transform.localPosition = Vector3.zero;
            npcManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget <= npcManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(NPCManager npcManager)
        {
            // Rotate manually
            if (npcManager.isPerformingAction)
            {
                Vector3 direction = npcManager.currentTarget.transform.position - npcManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = npcManager.transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                npcManager.transform.rotation = Quaternion.Slerp(npcManager.transform.rotation, targetRotation, npcManager.rotationSpeed / Time.deltaTime);
            }
            // Rotate with Navmesh (pathfinding)
            else
            {
                Vector3 relativeDirection = npcManager.transform.InverseTransformDirection(npcManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = npcManager.npcRigidbody.velocity;
                //Debug.Log(targetVelocity);

                npcManager.navMeshAgent.enabled = true;
                npcManager.navMeshAgent.SetDestination(npcManager.currentTarget.transform.position);
                npcManager.npcRigidbody.velocity = npcManager.navMeshAgent.desiredVelocity;
                npcManager.transform.rotation = Quaternion.Slerp(npcManager.transform.rotation, npcManager.navMeshAgent.transform.rotation, npcManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
