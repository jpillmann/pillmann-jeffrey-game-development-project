using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class AnimalFleeingState : AnimalState
    {
        public AnimalState animalIdleState;
        public override AnimalState Tick(AnimalManager animalManager, AnimalStats animalStats)
        {
            Vector3 targetDirection = animalManager.currentTarget.transform.position - animalManager.transform.position;
            float distanceFromTarget = Vector3.Distance(animalManager.currentTarget.transform.position, animalManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, animalManager.transform.forward);

            Vector3 fleeingDirection = -targetDirection;
            fleeingDirection.Normalize();
            Vector3 fleeingPosition = animalManager.transform.position + (fleeingDirection * 40);

            //if (distanceFromTarget < animalManager.minimumDesiredDistance)
            //{
            //    animalAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            //}

            HandleRotateAwayFromTarget(animalManager, fleeingPosition);

            animalManager.navMeshAgent.transform.localPosition = Vector3.zero;
            animalManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget >= animalManager.minimumDesiredDistance)
            {
                return animalIdleState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateAwayFromTarget(AnimalManager animalManager, Vector3 fleeingPosition)
        {
            Vector3 relativeDirection = animalManager.transform.InverseTransformDirection(animalManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = animalManager.animalRigidbody.velocity;

            animalManager.navMeshAgent.enabled = true;
            animalManager.navMeshAgent.SetDestination(fleeingPosition);
            animalManager.animalRigidbody.velocity = targetVelocity;
            animalManager.transform.rotation = Quaternion.Slerp(animalManager.transform.rotation, animalManager.navMeshAgent.transform.rotation, animalManager.rotationSpeed / Time.deltaTime);
        }
    }
}