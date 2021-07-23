using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    [System.Serializable]
    public class Tenant
    {
        public string title;
        public string description;
        public TenantType tenantType;
        public int favorReward;
        public int favorLoss;
    }

    public enum TenantType
    {
        Kill,
        Gather,
        Quest,
        Rule
    }
}
