using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class DamagePlayer : MonoBehaviour
    {
        private int damage = 25;

        // Damages Players Health Stat on Collision
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}
