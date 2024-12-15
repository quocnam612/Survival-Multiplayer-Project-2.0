using Photon.Pun;
using UnityEngine;

public class CuttableBush : MonoBehaviourPunCallbacks
{
    public string prefabPath;

    private InventorySystem inventorySystem;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit range") && inventorySystem.handHold.transform.childCount != 0)
        {
            PhotonNetwork.Instantiate(prefabPath, transform.position, transform.rotation);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
