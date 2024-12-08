using Photon.Pun;
using UnityEngine;

public class ItemRandomCall : MonoBehaviourPunCallbacks
{
    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    GameObject parentObject = info.Sender.TagObject as GameObject;

    //    if (parentObject != null)
    //    {
    //        transform.SetParent(parentObject.GetComponent<PlayerSetup>().handBone.transform, false);
    //        Debug.Log(parentObject.name);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Parent object not found for instantiation.");
    //    }
    //}


    [PunRPC]
    public void SetParentToSender(int actorNumber)
    {   
        GameObject playerObject = PlayerManager.GetPlayerObject(actorNumber);
        Transform handBone = playerObject.transform.Find("Armature/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1/Hand Global");
        transform.SetParent(handBone, false);
        Debug.Log($"Parented object to player {actorNumber}'s hand.");
    }
}