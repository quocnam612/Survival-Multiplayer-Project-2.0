using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviourPunCallbacks, IPunObservable
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
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthSlider.value = health;
        easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (photonView.IsMine)
        {
            UpdateHealth();
            photonView.RPC(nameof(SyncHealth), RpcTarget.Others, health);
        }
    }

    [PunRPC]
    public void SyncHealth(float newHealth)
    {
        health = newHealth;
        UpdateHealth();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            UpdateHealth();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(SyncHealth), newPlayer, health);
        }
    }
}
