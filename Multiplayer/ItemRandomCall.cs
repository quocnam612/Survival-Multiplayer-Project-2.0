using Photon.Pun;
using UnityEngine;

public class ItemRandomCall : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GameObject parentObject = PlayerManager.GetPlayerObject(info.Sender.ActorNumber);

        if (parentObject != null)
        {
            transform.SetParent(parentObject.GetComponent<PlayerSetup>().handBone.gameObject.transform, false);
            Debug.Log("Parent: " + parentObject.name);
        }
        else
        {
            Debug.LogWarning("Parent object not found for instantiation.");
        }
    }
}