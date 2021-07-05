using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            // Check for attack range
            // potentially circle play
            // if in attack range return attack state
            // if we are in a cool down after attacking, return this state and continue circling player
            // if player runs out of range return pursueTarget State
            float distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);

            if (npcManager.currentRecoveryTime <= 0 && distanceFromTarget <= npcManager.maximumAttackRange)
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
    }
}
