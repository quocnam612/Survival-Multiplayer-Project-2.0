using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        public string name;
        public int count = 0;
        public Item(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }

    [HideInInspector] public int capacity;
    [HideInInspector] public int countInventory = 0;
    public int maxStack = 50;
    public int maxInventorySlot = 50;
    public static InventorySystem Instance { get; set; }

    public GameObject inventorySlotsUI;
    public GameObject hotBarSlotsUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<Item> itemList = new List<Item>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isFull;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isFull = false;
        capacity = (maxInventorySlot + 10) * maxStack;
        PopulateSlotList();
    }

    private void PopulateSlotList() {
        foreach (Transform child in hotBarSlotsUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
                if (child.transform.childCount == 1)
                {
                    itemList.Add(new Item(child.transform.GetChild(0).gameObject.name, Int16.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text)));
                    countInventory += Int16.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                }
            }
        }

        foreach (Transform child in inventorySlotsUI.transform) {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
                if (child.transform.childCount == 1)
                {
                    itemList.Add(new Item(child.transform.GetChild(0).gameObject.name, Int16.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text)));
                    countInventory += Int16.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                }
            }
        }
    }

    public void AddToInventory(GameObject item) {
        if (CheckIfFull(item)) {
            isFull = true;

        }
        else if (FindNextSlot(item) != null) {
            isFull = false;
            whatSlotToEquip = FindNextSlot(item);
            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(item.GetComponent<InteractableObject>().GetItemName()), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation, whatSlotToEquip.transform);
            itemToAdd.transform.localScale = new Vector3(1, 1, 1);

            Item tmp = new Item(item.GetComponent<InteractableObject>().GetItemName(), Int16.Parse(itemToAdd.transform.GetChild(0).GetComponent<Text>().text));
            countInventory += Int16.Parse(itemToAdd.transform.GetChild(0).GetComponent<Text>().text);
            itemList.Add(tmp);
        }
        else {
            isFull = false;
        }
    }

    private GameObject FindNextSlot(GameObject item) {
        foreach(GameObject slot in slotList) {
            if (slot.transform.childCount == 1) {
                if (slot.transform.GetChild(0).gameObject.name == item.GetComponent<InteractableObject>().GetItemName() && Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) <= maxStack - 1) {
                    slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + 1).ToString();
                    countInventory++;
                    return null;
                }
            }
        }

        foreach (GameObject slot in slotList) {
            if (slot.transform.childCount == 0) return slot;
        }
        return null;
    }

    private bool CheckIfFull(GameObject item) {
        int counterSlot = 0;
        int counterItem = 0;

        foreach (GameObject slot in slotList) {
            if (slot.transform.childCount == 0) return false;
            else if (slot.transform.childCount == 1) {
                if (slot.transform.GetChild(0).gameObject.name == item.GetComponent<InteractableObject>().GetItemName()) {
                    counterItem += Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                }
                else counterSlot++;
            }
        }
        if ((counterSlot == maxInventorySlot + 10) || (counterItem == (maxInventorySlot + 10 - counterSlot) * maxStack)) return true;
        return false;
    }
}