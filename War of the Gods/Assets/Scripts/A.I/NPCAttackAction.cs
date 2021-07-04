using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    [CreateAssetMenu(menuName = "A.I/NPC Actions/Attack Action")]
    public class NPCAttackAction : NPCAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}
