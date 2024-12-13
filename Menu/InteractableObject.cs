using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviourPunCallbacks
{
    public string itemName;
    public bool pickable = true;
    public bool interactable = false;
    public bool stackable = true;
    public bool dropped = false;
    public int count = 1;

    public string GetItemName()
    {
        return itemName;  
    }

    [PunRPC]
    private void NetworkDestroy()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    private void SetItemName(string _name) {
        itemName = _name;
    }
}