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
        AnimatorHandler animatorHandler;
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

        public List<Quest> quests = new List<Quest>();
        public List<Quest> completedQuests = new List<Quest>();
        public Quest tempQuest;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInteracting;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;


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
        }

        // Reset Player Flags
        // Handle Camera Update
        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;

            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.inventory_Input = false;

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
    }
}
