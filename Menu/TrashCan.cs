using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    public AnnoucementDisplayer annoucement;
    public Sprite trashOpen;
    public Sprite trashClose;
    public InventorySystem inventory;
    public Image trashIcon {
        get {
            return gameObject.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (DragDrop.itemBeingDragged && RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), Input.mousePosition))
        {
            trashIcon.sprite = trashOpen;
        }
        else 
        {
            trashIcon.sprite = trashClose;
        }

        if (gameObject.transform.childCount == 1)
        { 
            if (inventory.checkInStringList(gameObject.transform.GetChild(0).gameObject.name, inventory.unstackableItems)) {
                annoucement.updateAction("Deleted " + gameObject.transform.GetChild(0).gameObject.name);
            }
            else {
                annoucement.updateAction("Deleted " + gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text + " " + gameObject.transform.GetChild(0).gameObject.name);
            }

            inventory.RemoveFromItemList(gameObject.transform.GetChild(0).gameObject.name, Int16.Parse(gameObject.transform.GetChild(0).GetComponent<DragDrop>().count.text));
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
}
