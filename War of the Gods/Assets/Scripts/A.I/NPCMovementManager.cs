using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class NPCMovementManager : MonoBehaviour
    {
        NPCManager npcManager;
        NPCAnimatorHandler npcAnimatorHandler;

        private void Awake()
        {
            npcManager = GetComponent<NPCManager>();
            npcAnimatorHandler = GetComponentInChildren<NPCAnimatorHandler>();
        }
    }
}
