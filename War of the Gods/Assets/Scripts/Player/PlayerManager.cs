using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        public AnimatorHandler animatorHandler;
        CameraHandler cameraHandler;
        PlayerMovement playerMovement;
        PlayerStats playerStats;
        PlayerInventory playerInventory;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableUIGameObject;

        public GameObject interactableUIDialogueObject;
        public GameObject interactableNPCName;
        public GameObject interactableNPCDialogue;

        public GameObject interactableUIQuestObject;
        public GameObject questTitle;
        public GameObject questDescription;

        public GameObject interactableUICompleteQuestObject;
        public GameObject completeQuestTitle;

        public GameObject interactableUIAltarObject;
        public GameObject altarTitleUIObject;
        public GameObject noviceBonusUIObject;
        public GameObject priestBonusUIObject;
        public GameObject championBonusUIObject;

        public List<Quest> quests = new List<Quest>();
        public List<Quest> completedQuests = new List<Quest>();
        public Quest tempQuest;

        public Bonus tempBonus;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isInteracting;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;
        public bool isBlocking;


        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        // Set Components
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerMovement = GetComponent<PlayerMovement>();
            playerStats = GetComponent<PlayerStats>();
            playerInventory = GetComponent<PlayerInventory>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        // Input Update [Interaction, Rolling, Sprinting]
        // Stamina Regen Update
        // Check for Interactable Update
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");

            anim.SetBool("isBlocking", isBlocking);

            inputHandler.TickInput(delta);
            playerMovement.HandleRollingAndSprinting(delta);
            playerStats.RegenerateStamina();

            CheckForInteractableObject();
        }

        // Handle Player Movement Update
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerMovement.HandleMovement(delta);
            playerMovement.HandleFalling(delta, playerMovement.moveDirection);
        }

        // Reset Player Flags
        // Handle Camera Update
        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.lb_Input = false;

            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.inventory_Input = false;
            inputHandler.y_Input = false;

            if (isInAir)
            {
                playerMovement.inAirTimer += Time.deltaTime;
            }

            float delta = Time.fixedDeltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        // Look for Interactable Object's around the Player with Spherecast
        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.6f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                // Set Interaction UI Element for "Interactable" and call Interact function
                #region Interactable
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                            animatorHandler.PlayTargetAnimation("PickUpItem", true);
                        }
                    }
                }
                #endregion

                // Set Interaction UI Element for "InteractableNPC" and call Interact function
                #region Interactable NPC
                else if (hit.collider.tag == "InteractableNPC")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null && !interactableUIDialogueObject.activeSelf)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                            interactableUIGameObject.SetActive(false);
                        }
                    }
                }
                #endregion

                else if (hit.collider.tag == "Altar")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null && !interactableUIAltarObject.activeSelf)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                            interactableUIGameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                // Close UI Interaction Window
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (interactableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                    interactableUIDialogueObject.GetComponent<DialogueTween>().HideDialogue();
                }
            }
        }

        // Sets the in tempQuest saved Quest to active and adds it to the Players Quest List
        public void AcceptQuest()
        {
            tempQuest.isActive = true;
            quests.Add(tempQuest);
            interactableUIQuestObject.GetComponent<QuestTween>().Close();
            tempQuest = null;
        }

        // Disables Quest Dialogue(Accept/Decline) Window and resets tempQuest variable
        public void DeclineQuest()
        {
            interactableUIQuestObject.GetComponent<QuestTween>().Close();
            tempQuest = null;
        }

        // Completes the Quest if the QuestGoal is Reached
        public void CompleteQuest()
        {
            for (int i = 0; i < quests.Count; i++)
            {
                if (quests[i].title == tempQuest.title)
                {
                    // Sets the currentAmount Value for the Quest
                    quests[i].questGoal.CheckCurrentAmount(playerInventory);

                    if (quests[i].questGoal.IsReached())
                    {
                        // Remove required Item(s) from Player Inventory
                        int currentAmount = quests[i].questGoal.requiredAmount;

                        for (int j = 0; j < playerInventory.weaponsInventory.Count; j++)
                        {
                            if (currentAmount > 0)
                            {
                                if (playerInventory.weaponsInventory[j] == quests[i].questGoal.item)
                                {
                                    currentAmount--;
                                    playerInventory.weaponsInventory.RemoveAt(j);
                                }
                            }
                            else
                            {
                                break;
                            }
                            
                        }

                        // Handle Quest Reward
                        playerInventory.weaponsInventory.Add(quests[i].weaponReward);

                        if (quests[i].questType == QuestType.SideQuest)
                        {
                            playerStats.sideQuestsCompleted++;
                        }
                        else if (quests[i].questType == QuestType.MainQuest)
                        {
                            playerStats.mainQuestsCompleted++;
                        }

                        itemInteractableUIGameObject.GetComponentInChildren<Text>().text = quests[i].weaponReward.itemName;
                        itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = quests[i].weaponReward.itemIcon.texture;

                        // Handle Quests Lists
                        quests.Remove(quests[i]);
                        tempQuest.isActive = false;
                        completedQuests.Add(tempQuest);
                        tempQuest = null;

                        // Handle UI
                        interactableUICompleteQuestObject.GetComponent<QuestTween>().Close();
                        itemInteractableUIGameObject.SetActive(true);
                        break;
                    }   
                }
            }
        }

        // Begin Worship of a God
        public void BeginWorship()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();

            if (playerStats.bonus == null)
            {
                playerStats.bonus = tempBonus;
                playerStats.faction = tempBonus.deity;
                playerStats.worshipTitle = "Novice";
                playerStats.favor = 30;

                if (tempBonus.bonusType == BonusType.Weapon)
                {
                    playerStats.swordDamageMultiplier = tempBonus.noviceBonus;
                    playerStats.axeDamageMultiplier = tempBonus.noviceBonus;
                    playerStats.maceDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Staff)
                {
                    playerStats.staffDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Dual)
                {
                    playerStats.dualWieldMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Magic)
                {
                    playerStats.magicDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Blood)
                {
                    playerStats.bloodDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Armor)
                {
                    playerStats.armorMultiplier = tempBonus.noviceBonus;
                }
            }
            else if (tempBonus.deity != playerStats.bonus.deity)
            {
                playerStats.bonus = tempBonus;
                playerStats.faction = tempBonus.deity;
                playerStats.worshipTitle = "Novice";
                playerStats.favor = 30;

                if (tempBonus.bonusType == BonusType.Weapon)
                {
                    playerStats.swordDamageMultiplier = tempBonus.noviceBonus;
                    playerStats.axeDamageMultiplier = tempBonus.noviceBonus;
                    playerStats.maceDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Staff)
                {
                    playerStats.staffDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Dual)
                {
                    playerStats.dualWieldMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Magic)
                {
                    playerStats.magicDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Blood)
                {
                    playerStats.bloodDamageMultiplier = tempBonus.noviceBonus;
                }
                else if (tempBonus.bonusType == BonusType.Armor)
                {
                    playerStats.armorMultiplier = tempBonus.noviceBonus;
                }
            }
        }
    }
}
