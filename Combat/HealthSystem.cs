using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviourPunCallbacks
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    public float lerpSpeed = 0.017f;

    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
    }

    private void Update()
    {
        GetComponent<PhotonView>().RPC(nameof(UpdateHealth), RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void UpdateHealth () {
        if (healthSlider.value != health) healthSlider.value = health; 
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    [PunRPC]
    void TakeDamage(float damage) {
        health -= damage;
    }
}
