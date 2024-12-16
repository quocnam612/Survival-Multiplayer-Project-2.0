using Photon.Pun;
using UnityEngine;

public class ChoppableCorp : MonoBehaviourPunCallbacks
{
    public HealthSystem healthSystem;
    public string prefabPath;
    public bool chopped = false;

    private InventorySystem inventorySystem;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit range") && inventorySystem.handHold.transform.childCount != 0)
        {
            float damage = inventorySystem.handHold.transform.childCount != 0 ? 15f : 3f;
            healthSystem.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBufferedViaServer, damage);

            if (healthSystem.health <= 0 && !chopped)
            {
                chopped = true;
                PhotonNetwork.Instantiate(prefabPath, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
