using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JP
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerMovement playerMovement;
        PlayerStats playerStats;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;


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
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");

            inputHandler.TickInput(delta);
            playerMovement.HandleMovement(delta);
            playerMovement.HandleRollingAndSprinting(delta);

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
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
                        // Set UI Text to Interactable Objects Text
                        //Set text pop up to true

                        if (inputHandler.a_Input)
                        {
                            Debug.Log("Oy!");
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
        }
    }
}
