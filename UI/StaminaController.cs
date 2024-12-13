using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;

public class StaminaController : MonoBehaviour {
    [Header("Stamina Main Parameters")]
    [SerializeField] private float playerStamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float minStamina = 20f;
    [SerializeField, Range(0.0001f, 1f)] private float tiredness = 0.5f;
    [SerializeField] private float jumpStamina = 17f;
    [HideInInspector] public bool isRegenerated = true;
    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public bool jumpable = true;

    [Header("Stamina Regenerate Parameters")]
    [Range(0, 50)][SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina UI Elements")]
    [SerializeField] private UnityEngine.UI.Image staminaProgesssUI = null;
    public Color maxStaminaColor = new Color(255, 255, 0);
    public Color minStaminaColor = new Color(255, 100, 0);
    
    public PlayerMovement2 playerController;
    public HungerController hungerController {
        get {
            return GetComponent<HungerController>();
        }
    }
    private float startStaminaRegen;

    private void Start() {
        playerStamina = maxStamina;
        startStaminaRegen = staminaRegen;
    }

    private void Update() {
        if (Input.GetButton("Jump") && playerController.grounded && playerController.readyToJump && isRegenerated && hungerController.playerHunger > 1f)
        {
            playerController.readyToJump = false;
            playerController.Jump();
            playerStamina -= jumpStamina;
            Invoke(nameof(ResetJump), playerController.jumpCooldown);
        }
        else if (playerController.state != PlayerMovement2.MovementState.sprinting) { 
            if (playerStamina < maxStamina && hungerController.playerHunger > 1f) {
                playerStamina += staminaRegen * Time.deltaTime;
                hungerController.hungerReduceDrainSprint();
        
                if (playerStamina >= minStamina) {
                    isRegenerated = true;
                    staminaRegen = startStaminaRegen;
                }
                else if (playerStamina < 1f){
                    isRegenerated = false;
                    staminaRegen = tiredness * startStaminaRegen;
                }
            }
        }
        if (Input.GetKey(playerController.sprintKey)) {
            if (playerStamina > 0 && isRegenerated) {
                playerController.readytoSprint = true;
                if (playerController.state == PlayerMovement2.MovementState.sprinting) {
                    playerStamina -= staminaDrain * Time.deltaTime;
                }
            }
            else
            {
                playerController.readytoSprint = false;
            }
        }

        if (hungerController.playerHunger > 1f) {
            hungerController.hungerReduceDrainNormal();
        }
        hungerController.UpdateHunger();
        UpdateStamina();
    }

    public void ResetJump()
    {
        playerController.readyToJump = true;
    }

    void UpdateStamina() {
        staminaProgesssUI.fillAmount = playerStamina / maxStamina;
        staminaProgesssUI.color = Color.Lerp(minStaminaColor, maxStaminaColor, Mathf.Pow(staminaProgesssUI.fillAmount, 1f / 3f) );
    }
}
