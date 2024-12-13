using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class CuttableBush : MonoBehaviourPunCallbacks
{
    public string prefabPath;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit range") && FindFirstObjectByType<InventorySystem>().handHold.transform.childCount != 0)
        {
            PhotonNetwork.Instantiate(prefabPath, transform.position, transform.rotation);
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
