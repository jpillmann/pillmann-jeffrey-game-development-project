using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JP
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
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


        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            playerStats = GetComponent<PlayerStats>();
            playerInventory = GetComponent<PlayerInventory>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");

            inputHandler.TickInput(delta);
            playerMovement.HandleRollingAndSprinting(delta);
            playerStats.RegenerateStamina();

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerMovement.HandleMovement(delta);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
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

        // Look for Interactable Object's around the Player
        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.6f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
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
                        }
                    }
                }
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
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (interactableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                    interactableUIDialogueObject.SetActive(false);
                }
            }
        }

        // Sets the in tempQuest saved Quest to active and adds it to the Players Quest List
        public void AcceptQuest()
        {
            tempQuest.isActive = true;
            quests.Add(tempQuest);
            interactableUIQuestObject.SetActive(false);
            tempQuest = null;
        }

        // Disables Quest Dialogue(Accept/Decline) Window and resets tempQuest variable
        public void DeclineQuest()
        {
            interactableUIQuestObject.SetActive(false);
            tempQuest = null;
        }

        // Completes the Quest if the QuestGoal is Reached
        public void CompleteQuest()
        {
            for (int i = 0; i < quests.Count; i++)
            {
                if (quests[i].title == tempQuest.title)
                {
                    quests[i].questGoal.CheckCurrentAmount(playerInventory);

                    if (quests[i].questGoal.IsReached())
                    {
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
                        interactableUICompleteQuestObject.SetActive(false);
                        itemInteractableUIGameObject.SetActive(true);
                        break;
                    }   
                }
            }
        }
    }
}
