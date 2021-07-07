using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JP
{
    public class AnimalIdleState : AnimalState
    {
        public AnimalFleeingState animalFleeingState;
        public LayerMask detectionLayer;

        public override AnimalState Tick(AnimalManager animalManager, AnimalStats animalStats)
        {
            float distanceFromTarget = animalManager.minimumDesiredDistance; ;
            Collider[] colliders = Physics.OverlapSphere(animalManager.transform.position, animalManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - animalManager.transform.position;
                    distanceFromTarget = Vector3.Distance(animalManager.currentTarget.transform.position, animalManager.transform.position);
                    float viewableAngle = Vector3.Angle(targetDirection, animalManager.transform.forward);

                    if (viewableAngle > animalManager.minimumDetectionAngle && viewableAngle < animalManager.maximumDetectionAngle)
                    {
                        animalManager.currentTarget = characterStats;
                    }
                }
            }

            if (animalManager.currentTarget != null && distanceFromTarget < animalManager.minimumDesiredDistance)
            {
                return animalFleeingState;
            }
            else
            {
                //animalAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }
        }
    }
}