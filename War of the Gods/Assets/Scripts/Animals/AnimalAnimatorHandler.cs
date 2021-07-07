using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class AnimalAnimatorHandler : AnimatorManager
    {
        AnimalManager animalManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            animalManager = GetComponentInParent<AnimalManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            animalManager.animalRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            animalManager.animalRigidbody.velocity = velocity;
        }
    }
}
