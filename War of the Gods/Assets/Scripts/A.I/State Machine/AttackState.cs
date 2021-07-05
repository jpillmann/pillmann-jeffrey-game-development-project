using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;

        public NPCAttackAction[] npcAttacks;
        public NPCAttackAction currentAttack;

        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            // Select one of our many attacks based on attack scores
            // if selected attack is not able to be used because of bad angle, or distance, select a new attack
            // if attack is viable, stop movement and attack our target
            // set recovery timer to the attacks recovery time
            // return to combat stance state
            Vector3 targetDirection = npcManager.currentTarget.transform.position - npcManager.transform.position;
            float distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, npcManager.transform.forward);

            if (npcManager.isPerformingAction)
                return combatStanceState;

            if (currentAttack != null)
            {
                // If we are too close to enemy to perform current attack, get new attack
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                // If we are close enough to attack, let us proceed
                else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                {
                    // If enemy is within our attacks viewable angle, we attack
                    if (viewableAngle <= currentAttack.maximumAttackAngle && viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if (npcManager.currentRecoveryTime <= 0 && npcManager.isPerformingAction == false)
                        {
                            npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            npcAnimatorHandler.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            npcAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            npcManager.isPerformingAction = true;
                            npcManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(npcManager);
            }

            return combatStanceState;
        }

        private void GetNewAttack(NPCManager npcManager)
        {
            Vector3 targetDirection = npcManager.currentTarget.transform.position - npcManager.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, npcManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(npcManager.currentTarget.transform.position, npcManager.transform.position);

            int maxScore = 0;

            for (int i = 0; i < npcAttacks.Length; i++)
            {
                NPCAttackAction npcAttackAction = npcAttacks[i];

                if (distanceFromTarget <= npcAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= npcAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= npcAttackAction.maximumAttackAngle && viewableAngle >= npcAttackAction.minimumAttackAngle)
                    {
                        maxScore += npcAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int j = 0; j < npcAttacks.Length; j++)
            {
                NPCAttackAction npcAttackAction = npcAttacks[j];

                if (distanceFromTarget <= npcAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= npcAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= npcAttackAction.maximumAttackAngle && viewableAngle >= npcAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        tempScore += npcAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            currentAttack = npcAttackAction;
                        }
                    }
                }
            }
        }
    }
}
