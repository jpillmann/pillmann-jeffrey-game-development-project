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
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;
        public bool inventory_Input;
        public bool rb_Input;
        public bool rt_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;
        public bool isInteracting;

        PlayerConttrols inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        UIManager uiManager;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;
        PlayerStats playerStats;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            uiManager = FindObjectOfType<UIManager>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerStats = GetComponent<PlayerStats>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        // Enable Input Actions
        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerConttrols();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.InventoryQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.InventoryQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.Interact.performed += i => a_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;

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
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
        }

        // Handle Movement Input
        private void HandleMoveInput(float delta)
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

        private void HandleAttackInput(float delta)
        {
            // RB Input handles RIGHT hand weapon's light attack
            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            // RT Input handles RIGHT hand weapon's heavy attack
            if (rt_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.SetCameraHeight();
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            // Switch Lock on Target Left
            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }
            // Switch Lock on Target Right
            else if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
        }
    }
}