using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;

public class SelectionManager : MonoBehaviourPunCallbacks
{
    public Text interaction_text;
    public Camera playerCamera;
    public float interaction_distance;
    public PlayerMovement2 playerMovement;
    public AnnoucementDisplayer annoucementDisplayer;
    public InventorySystem inventorySystem;
    public CraftingSystem craftingSystem;
    public bool readyToInteract, isCraft, isCook;

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interaction_distance, ~LayerMask.GetMask("Ignore Raycast"))) {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject>()) {
                interaction_text.enabled = true;

                if (selectionTransform.GetComponent<PlayerSetup>())
                {
                    if (selectionTransform.GetComponent<InteractableObject>().enabled) interaction_text.text = "<i>" + selectionTransform.GetComponent<InteractableObject>().GetItemName() + "</i>";
                    else interaction_text.enabled = false;
                }
                else if (selectionTransform.GetComponent<InteractableObject>().dropped) {
                    interaction_text.text = "[" + playerMovement.pickUpKey + "] Pick up dropped <b>" + selectionTransform.GetComponent<InteractableObject>().GetItemName() + "</b>";

                    if (Input.GetKeyDown(playerMovement.pickUpKey))
                    {
                        inventorySystem.AddToInventory(selectionTransform.gameObject, selectionTransform.GetComponent<InteractableObject>().count);
                        if (inventorySystem.isFull)
                        {
                            annoucementDisplayer.updateAction("Inventory is FULL");
                        }
                        else
                        {
                            if (selectionTransform.GetComponent<InteractableObject>().stackable) {
                                annoucementDisplayer.updateAction("Picked up " + selectionTransform.GetComponent<InteractableObject>().count + " " + hit.transform.gameObject.GetComponent<InteractableObject>().GetItemName());
                            }
                            else {
                                annoucementDisplayer.updateAction("Picked up 1" + hit.transform.gameObject.GetComponent<InteractableObject>().GetItemName());
                            }
                            hit.transform.gameObject.GetComponent<PhotonView>().RPC("NetworkDestroy", RpcTarget.AllBuffered);
                        }
                    }
                }
                else if (selectionTransform.GetComponent<InteractableObject>().pickable)
                {
                    interaction_text.text = "[" + playerMovement.pickUpKey + "] Pick up <b>" + selectionTransform.GetComponent<InteractableObject>().GetItemName() + "</b>";

                    if (Input.GetKeyDown(playerMovement.pickUpKey))
                    {
                        inventorySystem.AddToInventory(hit.transform.gameObject, 1);
                        if (inventorySystem.isFull)
                        {
                            annoucementDisplayer.updateAction("Inventory is FULL");
                        }
                        else
                        {
                            annoucementDisplayer.updateAction("Picked up 1 " + hit.transform.gameObject.GetComponent<InteractableObject>().GetItemName());
                            hit.transform.gameObject.GetComponent<PhotonView>().RPC("NetworkDestroy", RpcTarget.AllBuffered);
                        }
                    }
                }
                else
                {
                    interaction_text.text = "<b>" + selectionTransform.GetComponent<InteractableObject>().GetItemName() + "</b>";
                }


                if (selectionTransform.GetComponent<InteractableObject>().interactable) {
                    interaction_text.enabled = true;
                    interaction_text.text += "\n" + "[" + playerMovement.interactKey + "] Interact";
                    readyToInteract = true;
                    isCraft = false;
                    isCook = false;
                    switch (selectionTransform.GetComponent<InteractableObject>().GetItemName()) {
                        case "Crafting Table":
                            isCraft = true;
                            break;
                        case "Campfire":
                            isCook = true;
                            break;
                        default:
                            Debug.Log("INTERACTED");
                            break;
                    }
                }
                else {
                    readyToInteract = false;
                }
                
            }
            else {
                readyToInteract = false;
                interaction_text.enabled = false;
            }
        }
        else {
            readyToInteract = false;
            interaction_text.enabled = false;
        }
    }
}