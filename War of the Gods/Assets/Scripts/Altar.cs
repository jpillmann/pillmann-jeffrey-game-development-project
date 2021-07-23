using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class Altar : Interactable
    {
        public string altarTitle;
        public Tenant[] tenants;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            PrayAtAltar(playerManager);
        }

        private void PrayAtAltar(PlayerManager playerManager)
        {
            playerManager.animatorHandler.PlayTargetAnimation("KneelDown", true);

            playerManager.altarTitleUIObject.GetComponentInChildren<Text>().text = altarTitle;

            playerManager.firstTenantUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = tenants[0].title;
            playerManager.firstTenantUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = tenants[0].description;

            playerManager.secondTenantUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = tenants[1].title;
            playerManager.secondTenantUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = tenants[1].description;

            playerManager.thirdTenantUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = tenants[2].title;
            playerManager.thirdTenantUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = tenants[2].description;

            playerManager.interactableUIAltarObject.SetActive(true);
        }
    }
}
