using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;

public class HealthController : MonoBehaviour
{
    [Header("Health Main Parameters")]
    [SerializeField] private float playerHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minHealth = 20f;
    [HideInInspector] public bool isRegenerated = true;

    [Header("Health Regenerate Parameters")]
    [Range(0, 50)][SerializeField] private float healthDrain = 0.5f;
    [Range(0, 50)][SerializeField] private float healthRegen = 0.5f;

    [Header("Health UI Elements")]
    [SerializeField] private UnityEngine.UI.Image healthProgesssUI = null;
    public Color maxHealthColor = new Color(255, 0, 124);
    public Color minHealthColor = new Color(191, 0, 6);

    public PlayerMovement2 playerController;

    private void Start()
    {
        playerHealth = maxHealth;
    }

    private void Update()
    {

    }

    void UpdateHealth()
    {
        healthProgesssUI.fillAmount = playerHealth / maxHealth;
        healthProgesssUI.color = Color.Lerp(minHealthColor, maxHealthColor, Mathf.Pow(healthProgesssUI.fillAmount, 1f / 3f));
    }
}

