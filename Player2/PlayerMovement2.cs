using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SocialPlatforms;

namespace UnityStandardAssests.Character.FirstPerson
{
    public class PlayerMovement2 : MonoBehaviour
    {
        public CharacterController controller;

        [Header("Movement")]
        public MovementState state;
        public float moveSpeed;
        public float walkSpeed = 6.5f;
        public float sprintSpeed = 10f;
        public float airMultiplier = 1.2f;
        public int sprintFov = 70;
        public Camera playerCamera;
        public TextMeshProUGUI crosshairUI;
        [Range(0f, 100f)] public float speedChangeSpeed;
        [Range(0f, 100f)] public float fovChangeSpeed;
        [Range(0f, 10f)] public float sensChangeSpeed;
        [Range(0f, 10f)] public float cameraTiltChangeSpeed;
        [Range(0f, 10f)] public float cameraCrouchChangeSpeed;
        public float cameraTiltRange;
        public int zoomFov = 15;
        private float startFov;
        private bool isZooming = false;

        [Header("Jumping")]
        public float gravity = -30f;
        public float jumpHeight = 3f;
        public float jumpCooldown = 0.05f;

        [Header("Crouching")]
        public float crouchSpeed = 2f;

        [Header("Animation")]
        Animator animator;

        [Header("Keybinds")]
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;
        public KeyCode zoomKey = KeyCode.Z;
        public KeyCode attackKey = KeyCode.Mouse0;
        public KeyCode pickUpKey = KeyCode.F;
        public KeyCode openMenuKey = KeyCode.Tab;
        public KeyCode interactKey = KeyCode.E;

        [Header("Ground Check")]
        public Transform groundcheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        Vector3 velocity;
        [HideInInspector] public bool grounded;
        [HideInInspector] public bool readyToJump = true;
        [HideInInspector] public bool readytoSprint = true;
        [HideInInspector] public bool isCrouching = false;

        [Header("Menu")]
        public GameObject pauseMenu;
        public RectTransform hotBar;
        public GameObject reviewCamera;
        public GameObject interactUI;

        private MouseLook mouseLook;
        [HideInInspector] public float cameraTilt = 0;
        [HideInInspector] public int desireTilt;
        private float startMouseXSens;
        private float startMouseYSens;
        public enum MovementState {idling, walking, sprinting, air, crouching }
        private bool isMoving;
        private bool isAttack;
        float tmpmoveSpeed = 0;
        float leftright = 0;
        

        void Start()
        {
            pauseMenu.SetActive(false);
            reviewCamera.SetActive(false);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
            startFov = playerCamera.fieldOfView;
            mouseLook = playerCamera.GetComponent<MouseLook>();
            animator = GetComponent<Animator>();
            startMouseXSens = mouseLook.mouseXSensitivity;
            startMouseYSens = mouseLook.mouseYSensitivity;
        }

        void Update()
        {
            // Check ground and perform gravity
            StateHandler();
            grounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);
            if (grounded && velocity.y < 0) velocity.y = -2f;

            // Open pause menu
            if (Input.GetKeyDown(openMenuKey)) {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                reviewCamera.SetActive(!reviewCamera.activeSelf);
                interactUI.SetActive(!interactUI.activeSelf);
            }
            if (pauseMenu.activeSelf) {
                mouseLook.unlockMouse = true;
                hotBar.localScale = new Vector3(1.469f, 1.469f, 1.469f);
            }
            else {
                mouseLook.unlockMouse = false;
                hotBar.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }

            // Zooming UI crosshair
            if (isZooming) crosshairUI.text = "+"; 
            else crosshairUI.text = "•";

            // Move AWSD
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Player camera tilt
            if (Input.GetAxisRaw("Horizontal") == -1) {
                desireTilt = 1;
            }
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                desireTilt = -1;
            }
            else {
                desireTilt = 0;
            }
            cameraTilt = Mathf.SmoothDamp(cameraTilt, desireTilt * cameraTiltRange, ref cameraTiltChangeSpeed, 0.2f);
            playerCamera.transform.localRotation = Quaternion.Euler(mouseLook.xRotation, 0, cameraTilt);

            // Attack
            checkAttack();

            // Check move and crouch
            Vector3 move = transform.right * x + transform.forward * z;
            if (move == Vector3.zero) isMoving = false;
            else {
                controller.Move(move * moveSpeed * Time.deltaTime);
                isMoving = true;
            }
            if (Input.GetKeyDown(crouchKey) && !isCrouching)
            {
                isCrouching = true;
            }
            else if (Input.GetKeyUp(crouchKey) && isCrouching)
            {
                isCrouching = false;
            }
            if (state == MovementState.crouching) {
                // Move player camera
                float newPosition1 = Mathf.SmoothDamp(playerCamera.transform.position.y, groundcheck.transform.position.y + 1.11f, ref cameraCrouchChangeSpeed, 0.1f);
                playerCamera.transform.position = new Vector3(transform.position.x, newPosition1, transform.position.z);
                // Move player collider
                controller.center = new Vector3(controller.center.x, 0.7f, controller.center.z);
                controller.height = 1.45f;
                controller.radius = 0.45f;
            }
            else {
                // Move player camera
                float newPosition1 = Mathf.SmoothDamp(playerCamera.transform.position.y, groundcheck.transform.position.y + 1.61f, ref cameraCrouchChangeSpeed, 0.1f);
                playerCamera.transform.position = new Vector3(transform.position.x, newPosition1, transform.position.z);
                // Move player collider
                controller.center = new Vector3(controller.center.x, 0.9f, controller.center.z);
                controller.height = 1.8f;
                controller.radius = 0.3f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Animation based on variables
            if (z > 0)
            {
                adjustSpeed(-(moveSpeed), speedChangeSpeed);
                adjustLeftRight(Input.GetAxisRaw("Horizontal") / 2, speedChangeSpeed / 9f);
            }
            else if (z < 0)
            {
                adjustSpeed(moveSpeed, speedChangeSpeed);
                adjustLeftRight(Input.GetAxisRaw("Horizontal") / 2, speedChangeSpeed / 9f);
            }
            else {
                adjustSpeed(moveSpeed, speedChangeSpeed);
                adjustLeftRight(Input.GetAxisRaw("Horizontal"), speedChangeSpeed / 9f);
            }

            animator.SetFloat("Speed", tmpmoveSpeed / -8.9f);
            animator.SetFloat("Horizontal", leftright);
            animator.SetFloat("headTilt", mouseLook.xRotation);
            animator.SetBool("Jump", grounded);
            animator.SetBool("Crouch", isCrouching);
            animator.SetBool("Moving", isMoving);
            animator.SetBool("Zoom", isZooming);
            animator.SetBool("Attack", isAttack);
        }

        private void StateHandler()
        {
            if (Input.GetKey(crouchKey) && grounded)
            {
                state = MovementState.crouching;
                if (isMoving) moveSpeed = crouchSpeed;
                else moveSpeed = 0;
                checkZoom();
            }
            else if (grounded && !isMoving) {
                state = MovementState.idling;
                moveSpeed = 0;
                checkZoom();
            }
            else if (grounded && Input.GetKey(sprintKey) && readytoSprint && !isZooming)
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
                adjustFov(sprintFov, fovChangeSpeed);
            }
            else if (grounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
                checkZoom();
            }
            else
            {
                if (state == MovementState.sprinting) {
                    moveSpeed = sprintSpeed * airMultiplier;
                    adjustFov(sprintFov, fovChangeSpeed);
                }
                else if (state == MovementState.walking){
                    moveSpeed = walkSpeed * airMultiplier * 1.25f;
                    adjustFov(startFov, fovChangeSpeed);
                }
                else if (state == MovementState.crouching) {
                    moveSpeed = crouchSpeed * airMultiplier * 1.8f;
                    adjustFov(startFov, fovChangeSpeed);
                }
                state = MovementState.air;
            }
        }

        public void Jump() {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        private void checkAttack() {
            if (Input.GetKeyDown(attackKey))
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
        }

        private void recoverFov() {
            adjustFov(startFov, fovChangeSpeed * 5);
        }

        private void checkZoom() {
            if (Input.GetKey(zoomKey))
            {
                adjustFov(zoomFov, fovChangeSpeed * 5);
                mouseLook.mouseXSensitivity = zoomFov / startFov * startMouseXSens;
                mouseLook.mouseYSensitivity = zoomFov / startFov * startMouseYSens;
                isZooming = true;
            }
            else if (Input.GetKeyUp(zoomKey))
            {
                InvokeRepeating(nameof(recoverFov), 0, Time.deltaTime);
                mouseLook.mouseXSensitivity = startMouseXSens;
                mouseLook.mouseYSensitivity = startMouseYSens;
                isZooming = false;
            }
            else
            {
                adjustFov(startFov, fovChangeSpeed);
            }
        }

        private void adjustFov(float desireFov, float changeSpeed) {
            if (playerCamera.fieldOfView < desireFov - changeSpeed * Time.deltaTime * 1.1f)
            {
                playerCamera.fieldOfView += changeSpeed * Time.deltaTime;
            }
            else if (playerCamera.fieldOfView > desireFov + changeSpeed * Time.deltaTime * 1.1f)
            {
                playerCamera.fieldOfView -= changeSpeed * Time.deltaTime;
            }
            else { 
                CancelInvoke(nameof(recoverFov));
                playerCamera.fieldOfView = desireFov;
            }
        }

        private void adjustSpeed(float desireSpeed, float changeSpeed) {
            if (tmpmoveSpeed < desireSpeed - changeSpeed * Time.deltaTime * 1.1f)
            {
                tmpmoveSpeed += changeSpeed * Time.deltaTime;
            }
            else if (tmpmoveSpeed > desireSpeed + changeSpeed * Time.deltaTime * 1.1f)
            {
                tmpmoveSpeed -= changeSpeed * Time.deltaTime;
            }
            else {
                tmpmoveSpeed = desireSpeed;
            }
        }

        private void adjustLeftRight(float desireTurn, float changeSpeed) {
            if (leftright < desireTurn - changeSpeed * Time.deltaTime * 1.1f)
            {
                leftright += changeSpeed * Time.deltaTime;
            }
            else if (leftright > desireTurn + changeSpeed * Time.deltaTime * 1.1f)
            {
                leftright -= changeSpeed * Time.deltaTime;
            }
            else
            {
                leftright = desireTurn;
            }
        }
    }
}