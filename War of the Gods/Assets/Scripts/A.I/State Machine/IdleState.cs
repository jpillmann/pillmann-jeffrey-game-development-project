using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public FleeingState fleeingState;
        public LayerMask detectionLayer;

        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            #region Handle Target Detection

            Collider[] colliders = Physics.OverlapSphere(npcManager.transform.position, npcManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // CHECK FOR TEAM ID

                    Vector3 targetDirection = characterStats.transform.position - npcManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, npcManager.transform.forward);

                    if (viewableAngle > npcManager.minimumDetectionAngle && viewableAngle < npcManager.maximumDetectionAngle)
                    {
                        npcManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle Wandering

            if (npcManager.isPassive == false)
            {
                if (npcManager.isPerformingAction)
                {
                    npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }

                Vector3 wanderGoal = RandomNavMeshLocation(npcManager);

                if (npcManager.isWandering == false)
                {
                    npcManager.wanderingTargetLocation = wanderGoal;
                    npcManager.isWandering = true;
                }

                Vector3 wanderDirection = wanderGoal - npcManager.transform.position;
                float distanceFromGoal = Vector3.Distance(wanderGoal, npcManager.transform.position);

                if (distanceFromGoal > npcManager.navMeshAgent.stoppingDistance)
                {
                    npcAnimatorHandler.anim.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
                }
                else
                {
                    npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    npcManager.wanderingTargetLocation = Vector3.zero;
                    npcManager.isWandering = false;
                }

                HandleRotation(npcManager, wanderGoal);

                npcManager.navMeshAgent.transform.localPosition = Vector3.zero;
                npcManager.navMeshAgent.transform.localRotation = Quaternion.identity;
            }
            #endregion

            #region Handle Switch State

            if (npcManager.currentTarget != null && npcManager.isEnemy == true)
            {
                return pursueTargetState;
            }
            else if (npcManager.currentTarget != null && npcManager.isEnemy == false)
            {
                return fleeingState;
            }
            else
            {
                return this;
            }
            #endregion
        }

        private Vector3 RandomNavMeshLocation(NPCManager npcManager)
        {
            if (npcManager.wanderingTargetLocation == Vector3.zero)
            {
                Vector3 finalPosition = Vector3.zero;
                Vector3 randomPosition = Random.insideUnitSphere * npcManager.walkradius;
                randomPosition += npcManager.transform.position;

                if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, npcManager.walkradius, 1))
                {
                    finalPosition = hit.position;
                }
                return finalPosition;
            }
            else
            {
                return npcManager.wanderingTargetLocation;
            }
        }

        private void HandleRotation(NPCManager npcManager, Vector3 wanderGoal)
        {
            // Rotate manually
            if (npcManager.isPerformingAction)
            {
                Vector3 direction = wanderGoal - npcManager.transform.position;
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

                npcManager.navMeshAgent.enabled = true;
                npcManager.navMeshAgent.SetDestination(wanderGoal);
                npcManager.npcRigidbody.velocity = targetVelocity;
                npcManager.transform.rotation = Quaternion.Slerp(npcManager.transform.rotation, npcManager.navMeshAgent.transform.rotation, npcManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
