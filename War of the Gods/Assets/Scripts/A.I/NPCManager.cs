using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class NPCManager : CharacterManager
    {
        NPCMovementManager npcMovementManager;
        NPCAnimatorHandler npcAnimatorHandler;
        NPCStats npcStats;


        public State currentState;
        public CharacterStats currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody npcRigidbody;

        public bool isPerformingAction;
        public float rotationSpeed = 25;
        public float maximumAttackRange = 1.5f;

        public bool isWandering = false;
        public Vector3 wanderingTargetLocation = Vector3.zero;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public bool isPassive;
        [Range(1, 100)] public float walkradius; 

        // A.I Detection FOV
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;


        private void Awake()
        {
            npcMovementManager = GetComponent<NPCMovementManager>();
            npcAnimatorHandler = GetComponentInChildren<NPCAnimatorHandler>();
            npcStats = GetComponent<NPCStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            npcRigidbody = GetComponent<Rigidbody>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            npcRigidbody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, npcStats, npcAnimatorHandler);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }
}
