using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public abstract class AnimalState : MonoBehaviour
    {
        public abstract AnimalState Tick(AnimalManager animalManager, AnimalStats animalStats);
    }
}