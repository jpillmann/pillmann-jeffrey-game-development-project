using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class Altar : Interactable
    {
        public string altarTitle;
        public Bonus bonus;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            PrayAtAltar(playerManager);
        }

        private void PrayAtAltar(PlayerManager playerManager)
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();

            playerManager.animatorHandler.PlayTargetAnimation("KneelDown", true);
            playerStats.HandleFavor();

            playerManager.altarTitleUIObject.GetComponentInChildren<Text>().text = altarTitle;

            playerManager.noviceBonusUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = bonus.noviceBonusTitle;
            playerManager.noviceBonusUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = bonus.noviceBonusDescription;

            playerManager.priestBonusUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = bonus.priestBonusTitle;
            playerManager.priestBonusUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = bonus.priestBonusDescription;

            playerManager.championBonusUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = bonus.championBonusTitle;
            playerManager.championBonusUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = bonus.championBonusDescription;

            playerManager.tempBonus = bonus;

            playerManager.interactableUIAltarObject.SetActive(true);
        }
    }
}
