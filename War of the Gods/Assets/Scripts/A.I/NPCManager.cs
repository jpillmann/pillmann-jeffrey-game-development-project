using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCManager : CharacterManager
    {
        NPCMovementManager npcMovementManager;
        public bool isPerformingAction;

        [Header("A.I Settings")]
        public float detectionRadius = 20;

        // A.I Detection FOV
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        private void Awake()
        {
            npcMovementManager = GetComponent<NPCMovementManager>();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (npcMovementManager.currentTarget == null)
            {
                npcMovementManager.HandleDetection();
            }
            else
            {
                npcMovementManager.HandleMoveToTarget();
            }
        }
    }
}
