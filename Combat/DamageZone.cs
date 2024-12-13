using Photon.Pun;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage;

    private void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<HealthController>().reduceHealth(damage);
        }
    }
}
