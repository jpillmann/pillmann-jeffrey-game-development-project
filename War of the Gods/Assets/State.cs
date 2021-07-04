using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP {
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(NPCManager npcManager, NPCStats npcStats, NPCAnimatorHandler npcAnimatorHandler);
    }
}
