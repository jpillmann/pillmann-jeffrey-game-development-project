using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool inventory_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool sprintFlag;
        public bool inventoryFlag;
        public float rollInputTimer;
        public bool isInteracting;

        PlayerConttrols inputActions;
        PlayerInventory playerInventory;
        UIManager uiManager;
        PlayerManager playerManager;
        PlayerStats playerStats;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerInventory = GetComponent<PlayerInventory>();
            uiManager = FindObjectOfType<UIManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
        }

        // Enable Input Actions
        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerConttrols();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.InventoryQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.InventoryQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.Interact.performed += i => a_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        // Check for Input
        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
        }

        // Handle Movement Input
        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        // Handle roll Input
        // Rolling and Sprinting only possible with enough stamina
        // Sprint only when the Player is moving
        private void HandleRollInput(float delta)
        {

            if (b_Input)
            {
                rollInputTimer += delta;

                if (playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }
     
        // Quick Slot Input to swap between equipped weapons
        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        // Opens Select Window [Player can choose between Quest, Inventory, Equip, Settings]
        // Disables HUD while Select Window is open
        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }
    }
}