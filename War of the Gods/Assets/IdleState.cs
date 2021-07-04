using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;

        public override State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler)
        {
            #region Handle Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, npcManager.detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // CHECK FOR TEAM ID

                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > npcManager.minimumDetectionAngle && viewableAngle < npcManager.maximumDetectionAngle)
                    {
                        npcManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle Switch State
            if (npcManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}
