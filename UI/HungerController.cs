using UnityEngine;

public class HungerController : MonoBehaviour
{
    [Header("Hunger Main Parameters")]
    [SerializeField] public float playerHunger = 100f;
    [SerializeField] private float maxHunger = 100f;

    [Header("Hunger Add Parameters")]
    [Range(0, 50)][SerializeField] private float hungerDrain = 0.2f;
    [Range(0f, 10f)][SerializeField] private float hungerDrainMultiplier = 1f;
    [Range(0f, 10f)][SerializeField] private float hungerAddMultiplier = 1f;

    [Header("Stamina UI Elements")]
    [SerializeField] private UnityEngine.UI.Image hungerProgesssUI = null;
    public Color maxHungerColor = new Color(255, 255, 0);
    public Color minHungerColor = new Color(255, 100, 0);

    void Start()
    {
        playerHunger = maxHunger;
    }

    public void hungerReduceDrainNormal() {
        playerHunger -= hungerDrain * Time.deltaTime;
        GetComponent<HealthController>().regenerateHealth();
    }

    public void hungerReduceDrainSprint()
    {
        playerHunger -= hungerDrain * Time.deltaTime * hungerDrainMultiplier;
    }

    public void hungerAdd(float amount) {
        playerHunger += amount * hungerAddMultiplier;
        playerHunger = Mathf.Clamp(playerHunger, 0f, maxHunger + 100f);
        UpdateHunger();
    }

    public void UpdateHunger()
    {
        hungerProgesssUI.fillAmount = playerHunger / maxHunger;
        hungerProgesssUI.color = Color.Lerp(minHungerColor, maxHungerColor, Mathf.Pow(hungerProgesssUI.fillAmount, 1f / 3f));
    }

}
