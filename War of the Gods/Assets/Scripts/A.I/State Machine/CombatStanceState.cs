using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public IdleState idleState;

        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            // Check for attack range
            // potentially circle play
            // if in attack range return attack state
            // if we are in a cool down after attacking, return this state and continue circling player
            // if player runs out of range return pursueTarget State
            float distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);

            HandleRotateTowardsTarget(npcManager);

            if (npcManager.isPerformingAction)
            {
                npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            if (npcManager.currentTarget.isDead)
            {
                npcManager.currentTarget = null;
                return idleState;
            }
            else if (npcManager.currentRecoveryTime <= 0 && distanceFromTarget <= npcManager.maximumAttackRange)
            {
                return attackState;
            }
            else if (distanceFromTarget > npcManager.maximumAttackRange)
            {
                return pursueTargetState;
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
