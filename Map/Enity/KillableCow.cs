using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class KillableCow : MonoBehaviourPunCallbacks
{
    public HealthSystem healthSystem;
    public string prefabPath;
    public bool cowDied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit range") && FindFirstObjectByType<InventorySystem>().handHold.transform.childCount != 0)
        {
            if (FindFirstObjectByType<InventorySystem>().handHold.transform.GetChild(0).gameObject.name.Contains("Pickaxe"))
            {
                healthSystem.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBufferedViaServer, 15f);
            }
            else
            {
                healthSystem.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBufferedViaServer, 3f);
            }
            GetComponent<AI_Movement>().animator.SetTrigger("Hurt");
            if (healthSystem.health <= 0 && !cowDied) {
                cowDied = true;
                PhotonNetwork.Instantiate(prefabPath, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
