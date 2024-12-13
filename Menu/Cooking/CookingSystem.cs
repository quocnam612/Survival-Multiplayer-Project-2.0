using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookingSystem : MonoBehaviour
{
    [HideInInspector] public InventorySystem inventorySystem {
        get {
            return GetComponent<InventorySystem>();
        }
    }
    public UnityEngine.UI.Image progressInicator;
    public GameObject cookSlot;
    public GameObject cookedSlot;
    public FuelSlot fuelSlot;
    public GameObject iconContainer;
    public float countDown = 0f;
    public float cookTime = 1f;
    public bool isCooking;

    private void LateUpdate()
    {
        handleCountDown();
    }

    public void handleCountDown() {
        if (fuelSlot.burnTime > 0f && cookSlot.transform.childCount != 0 && cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookable &&
        ( (cookedSlot.transform.childCount == 0) || (cookedSlot.transform.childCount != 0 && cookedSlot.transform.GetChild(0).name == cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookedItem && Int16.Parse(cookedSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) != inventorySystem.maxStack) ) )
        {
            isCooking = true;
            if (countDown > 0f)
            {
                countDown -= Time.deltaTime;
                fuelSlot.burnFuel();
            }
            else {
                if (cookedSlot.transform.childCount == 0)
                {
                    GameObject itemCooked = Instantiate(Resources.Load<GameObject>("Item Slot/" + cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookedItem), cookedSlot.transform.position, cookedSlot.transform.rotation, cookedSlot.transform);
                    itemCooked.name = cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookedItem;
                }
                else
                {
                    cookedSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(cookedSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + 1).ToString();
                }

                if (Int16.Parse(cookSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) == 1)
                {
                    Destroy(cookSlot.transform.GetChild(0).gameObject);
                    iconContainer.SetActive(false);
                }
                else
                {
                    cookSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(cookSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) - 1).ToString();
                    startCook();
                }

                
            }
        }
        else {
            countDown = 0f;
            isCooking = false;
        }

        progressInicator.fillAmount = 1 - countDown / cookTime;
    }

    public void startCook() {
        if (fuelSlot.burnTime > 0f && cookSlot.transform.childCount != 0 && cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookable &&
        ((cookedSlot.transform.childCount == 0) || (cookedSlot.transform.childCount != 0 && cookedSlot.transform.GetChild(0).name == cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookedItem && Int16.Parse(cookedSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) != inventorySystem.maxStack)))
        {
            cookTime = cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookTime;
            countDown = cookTime;
            iconContainer.SetActive(true);
            iconContainer.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + cookSlot.transform.GetChild(0).GetComponent<DragDrop>().cookedItem);
        }
        else {
            countDown = 0f;
            iconContainer.SetActive(false);
        }
    }
}
