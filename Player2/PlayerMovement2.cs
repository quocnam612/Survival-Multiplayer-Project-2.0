using System.Data.SqlTypes;
using UnityEngine;

namespace UnityStandardAssests.Character.FirstPerson
{
    public class PlayerMovement2 : MonoBehaviour
    {
        public CharacterController controller;

        [Header("Movement")]
        public float moveSpeed;
        public float walkSpeed = 6.5f;
        public float sprintSpeed = 10f;
        public float airMultiplier = 1.2f;

        [Header("Jumping")]
        public float gravity = -30f;
        public float jumpHeight = 3f;
        public float jumpCooldown = 0.05f;

        [Header("Crouching")]
        public float crouchSpeed = 2f;
        [Range(0f, 1f)]
        public float crouchYScale = 0.5f;
        private float startYScale = 1f;

        [Header("Keybinds")]
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Ground Check")]
        public Transform groundcheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        Vector3 velocity;
        [HideInInspector] public bool grounded;
        [HideInInspector] public bool readyToJump = true;
        [HideInInspector] public bool readytoSprint = true;

        public MovementState state;
        public enum MovementState { walking, sprinting, air, crouching }

        void Start()
        {
            startYScale = transform.localScale.y;
        }

        void Update()
        {
            StateHandler();
            grounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

            if (grounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * moveSpeed * Time.deltaTime);


            if (Input.GetKeyDown(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            }
            if (Input.GetKeyUp(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void StateHandler()
        {
            if (Input.GetKey(crouchKey) && grounded)
            {
                state = MovementState.crouching;
                moveSpeed = crouchSpeed;
            }
            else if (grounded && Input.GetKey(sprintKey) && readytoSprint)
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
            }
            else if (grounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
            else
            {
                if (state == MovementState.sprinting) {
                    moveSpeed = sprintSpeed * airMultiplier;
                }
                else if (state == MovementState.walking){
                    moveSpeed = walkSpeed * airMultiplier * 1.25f;
                }
                else if (state == MovementState.crouching) {
                    moveSpeed = crouchSpeed * airMultiplier * 1.8f;
                }
                state = MovementState.air;
            }
        }

        public void Jump() {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        public void CancelJump() {
            CancelInvoke();
        }
    }
}