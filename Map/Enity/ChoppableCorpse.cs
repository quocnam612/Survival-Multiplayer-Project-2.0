using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class ChoppableCorp : MonoBehaviourPunCallbacks
{
    public HealthSystem healthSystem;
    public string prefabPath;
    public bool chopped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit range") && FindFirstObjectByType<InventorySystem>().handHold.transform.childCount != 0)
        {
            if (FindFirstObjectByType<InventorySystem>().handHold.transform.childCount != 0)
            {
                healthSystem.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBufferedViaServer, 15f);
            }
            else
            {
                healthSystem.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBufferedViaServer, 3f);
            }

            if (healthSystem.health <= 0 && !chopped) {
                chopped = true;
                PhotonNetwork.Instantiate(prefabPath, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
