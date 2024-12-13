using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;

public class HealthController : MonoBehaviour
{
    [Header("Health Main Parameters")]
    [SerializeField] private float playerHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minHealth = 20f;

    [Header("Health Regenerate Parameters")]
    [Range(0, 50)][SerializeField] private float healthRegen = 0.5f;

    [Header("Health UI Elements")]
    [SerializeField] private UnityEngine.UI.Image healthProgesssUI = null;
    public Color maxHealthColor = new Color(255, 0, 124);
    public Color minHealthColor = new Color(191, 0, 6);
    public GameObject lowHealthScreen;

    private void Start()
    {
        playerHealth = maxHealth;
    }

    public void reduceHealth(float amount) {
        playerHealth = Mathf.Clamp(playerHealth - amount, -1f, maxHealth);
        transform.root.GetComponent<PlayerMovement2>().animator.SetTrigger("Hit");
        UpdateHealth();
    }

    public void drainHealth(float amount) {
        playerHealth = Mathf.Clamp(playerHealth - amount * Time.deltaTime, -1f, maxHealth);
        transform.root.GetComponent<PlayerMovement2>().animator.SetTrigger("DrainHit");
        UpdateHealth();
    }

    public void regenerateHealth() {
        if (playerHealth < maxHealth) {
            playerHealth += healthRegen * Time.deltaTime;
            UpdateHealth() ;
        }
    }

    public void addHealth(float amount) {
        playerHealth = Mathf.Clamp(playerHealth + amount, -1f, maxHealth);
        UpdateHealth();
    }

    void UpdateHealth()
    {
        healthProgesssUI.fillAmount = playerHealth / maxHealth;
        healthProgesssUI.color = Color.Lerp(minHealthColor, maxHealthColor, Mathf.Pow(healthProgesssUI.fillAmount, 1f / 3f));
        if (playerHealth <= 0f) {
            transform.root.GetComponent<PlayerMovement2>().animator.runtimeAnimatorController = Resources.Load("Die") as RuntimeAnimatorController;
            transform.root.GetComponent<PlayerMovement2>().playerDied = true;
        }
        else if (playerHealth < minHealth) {
            lowHealthScreen.SetActive(true);
        }
        else {
            lowHealthScreen.SetActive(false);
        }
    }
}

