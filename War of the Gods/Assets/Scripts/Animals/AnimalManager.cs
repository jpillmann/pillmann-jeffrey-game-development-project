using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class AnimalManager : CharacterManager
    {
        //AnimalAnimatorHandler animalAnimatorHandler;
        AnimalStats animalStats;

        public CharacterStats currentTarget;
        public AnimalState currentState;
        public NavMeshAgent navMeshAgent;
        public Rigidbody animalRigidbody;

        public float rotationSpeed = 30;
        public float minimumDesiredDistance = 30;

        [Header("Animal Settings")]
        public float detectionRadius = 18;

        // Animal Detection FOV
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;


        private void Awake()
        {
            //animalAnimatorHandler = GetComponentInChildren<AnimalAnimatorHandler>();
            animalStats = GetComponent<AnimalStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            animalRigidbody = GetComponent<Rigidbody>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            animalRigidbody.isKinematic = false;
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                AnimalState nextState = currentState.Tick(this, animalStats);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(AnimalState state)
        {
            currentState = state;
        }
    }
}
