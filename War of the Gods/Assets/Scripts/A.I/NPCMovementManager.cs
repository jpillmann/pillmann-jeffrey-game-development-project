using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class NPCMovementManager : MonoBehaviour
    {
        NPCManager npcManager;
        NPCAnimatorHandler npcAnimatorHandler;
        NavMeshAgent navMeshAgent;

        public Rigidbody npcRigidbody;
        public CharacterStats currentTarget;
        public LayerMask detectionLayer;

        public float distanceFromTarget;
        public float stoppingDistance = 1f;

        public float rotationSpeed = 25;

        private void Awake()
        {
            npcManager = GetComponent<NPCManager>();
            npcAnimatorHandler = GetComponentInChildren<NPCAnimatorHandler>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            npcRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            npcRigidbody.isKinematic = false;
        }

        public void HandleDetection()
        {
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
                        currentTarget = characterStats;
                    }
                }
            }
        }

        public void HandleMoveToTarget()
        {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            // if npc is performing action: stop movement
            if (npcManager.isPerformingAction)
            {
                npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    npcAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    npcAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }

            HandleRotateTowardsTarget();

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotateTowardsTarget()
        {
            // Rotate manually
            if (npcManager.isPerformingAction)
            {
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            }
            // Rotate with Navmesh (pathfinding)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = npcRigidbody.velocity;

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
                npcRigidbody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}
