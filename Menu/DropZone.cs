using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviourPunCallbacks
{
    public AnnoucementDisplayer annoucement;
    public InventorySystem inventory;
    public Transform dropSpawner;

    private void Update()
    {
        if (gameObject.transform.childCount == 1)
        { 
            int count = 1;
            if (inventory.checkInStringList(gameObject.transform.GetChild(0).gameObject.name, inventory.unstackableItems)) {
                annoucement.updateAction("Dropped " + gameObject.transform.GetChild(0).gameObject.name);
            }
            else {
                count = Int16.Parse(gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                annoucement.updateAction("Dropped " + gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text + " " + gameObject.transform.GetChild(0).gameObject.name);
            }

            inventory.RemoveFromItemList(gameObject.transform.GetChild(0).gameObject.name, Int16.Parse(gameObject.transform.GetChild(0).GetComponent<DragDrop>().count.text));
            GameObject itemDropped = PhotonNetwork.Instantiate("Droppable/" + gameObject.transform.GetChild(0).gameObject.name, dropSpawner.position, dropSpawner.rotation);
            itemDropped.GetComponent<InteractableObject>().dropped = true;
            itemDropped.GetComponent<InteractableObject>().count = count;
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
}
