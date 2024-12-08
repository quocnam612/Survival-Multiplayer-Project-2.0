using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public InventorySystem inventorySystem
    {
        get
        {
            return GetComponentInParent<InventorySystem>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Item && DragDrop.itemBeingDragged)
        {
            if (DragDrop.startParent.childCount == 1 && Item.name != DragDrop.itemBeingDragged.gameObject.name) {
                return;
            }
            else if (Item.name != DragDrop.itemBeingDragged.gameObject.name || (Item.name == DragDrop.itemBeingDragged.gameObject.name && Mathf.Max(Int16.Parse(Item.transform.GetChild(0).GetComponent<Text>().text), Int16.Parse(DragDrop.itemBeingDragged.transform.GetChild(0).GetComponent<Text>().text)) == inventorySystem.maxStack)) {
                Item.transform.SetParent(DragDrop.startParent);
                DragDrop.startParent.GetChild(0).gameObject.transform.localPosition = new Vector2(0, 0);
            }
            else {
                int total = Int16.Parse(Item.transform.GetChild(0).GetComponent<Text>().text) + Int16.Parse(DragDrop.itemBeingDragged.transform.GetChild(0).GetComponent<Text>().text);
                if (total > inventorySystem.maxStack) {
                    Item.transform.GetChild(0).GetComponent<Text>().text = inventorySystem.maxStack.ToString();
                    DragDrop.itemBeingDragged.transform.GetChild(0).GetComponent<Text>().text = (total - inventorySystem.maxStack).ToString();
                }
                else {
                    Item.transform.GetChild(0).GetComponent<Text>().text = total.ToString();
                    Destroy(DragDrop.itemBeingDragged);
                }
                return;
            }
        }
        if (DragDrop.itemBeingDragged) {
            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
        }
    }
}