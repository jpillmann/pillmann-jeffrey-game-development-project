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
            npcManager.distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, npcManager.transform.forward);

            Vector3 fleeingDirection = -targetDirection;
            fleeingDirection.Normalize();
            Vector3 fleeingPosition = npcManager.transform.position + (fleeingDirection * 5);

            // Detecting Flock Neighbours
            Collider[] colliders = Physics.OverlapSphere(npcManager.transform.position, npcManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // CHECK FOR TEAM ID
                    if (characterStats.tag == npcManager.faction && characterStats.transform.position != npcManager.transform.position)
                    {
                        NPCManager nearestFlockNeighbour = characterStats.GetComponent<NPCManager>();
                        
                        if (nearestFlockNeighbour.distanceFromTarget > npcManager.distanceFromTarget)
                        {
                            npcManager.flockNeighbour = characterStats;
                            fleeingPosition = characterStats.transform.position;
                        }
                    }
                }
            }

            if (npcManager.distanceFromTarget < npcManager.minimumDesiredDistance)
            {
                npcAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateAwayFromTarget(npcManager, fleeingPosition);

            npcManager.navMeshAgent.transform.localPosition = Vector3.zero;
            npcManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (npcManager.distanceFromTarget >= npcManager.minimumDesiredDistance)
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