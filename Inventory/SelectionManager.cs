using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;

public class SelectionManager : MonoBehaviour
{
    public Text interaction_text;
    public Camera playerCamera;
    public float interaction_distance;
    public PlayerMovement2 playerMovement;
    public AnnoucementDisplayer annoucementDisplayer;

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interaction_distance) ) {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject>()) {
                if (selectionTransform.GetComponent<InteractableObject>().pickable) {
                    interaction_text.enabled = true;
                    interaction_text.text = "[" + playerMovement.pickUpKey + "] " + selectionTransform.GetComponent<InteractableObject>().GetItemName();

                    if (Input.GetKeyDown(playerMovement.pickUpKey))
                    {
                        InventorySystem.Instance.AddToInventory(hit.transform.gameObject);
                        if (InventorySystem.Instance.isFull)
                        {
                            annoucementDisplayer.updateAction("Inventory is FULL");
                        }
                        else
                        {
                            annoucementDisplayer.updateAction("Picked up 1 " + hit.transform.gameObject.GetComponent<InteractableObject>().GetItemName());
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
                else {
                    interaction_text.enabled = true;
                    interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                }
            }
            else {
                interaction_text.enabled = false;
            }

        }
        else {
            interaction_text.enabled = false;
        }
    }
}