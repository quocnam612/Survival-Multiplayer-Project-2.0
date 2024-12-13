using Photon.Pun;
using UnityEngine;

public class DamageZoneDrain : MonoBehaviour
{
    public int damagePerSec;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.GetComponentInChildren<HealthController>().drainHealth(damagePerSec);
        }
    }
}
