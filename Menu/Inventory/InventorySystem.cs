using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;
using static UnityEngine.UI.Image;

public class InventorySystem : MonoBehaviourPunCallbacks
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

    [Range(-1f, 11f)] public float selectedSlotIndex = 0f;
    [Range(0f,10f)] public float selectionScrollSpeed = 5f;
    [Range(0f, 1f)] public float swayClamp = 0.001f;
    [Range(0f, 3f)] public float smoothSway = 0.2f;

    [HideInInspector] public int capacity;
    [HideInInspector] public int countInventory = 0;
    [HideInInspector] public GameObject itemHold;
    [HideInInspector] public GameObject itemHoldGlobal;
    public int maxStack = 50;
    public int maxInventorySlot = 50;

    public GameObject inventoryStorage;
    public GameObject inventorySlotsUI;
    public GameObject hotBarSlotsUI;
    public GameObject selectedSlotUI;
    public GameObject selectSlotsUI;
    public GameObject handHoldGlobal;
    public GameObject handHold;

    public List<GameObject> slotList = new List<GameObject>();
    public List<Item> itemList = new List<Item>();
    public List<string> unstackableItems = new List<string>();
    public List<string> foodItem = new List<string>();
    public List<string> buildItem = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isFull;
    public bool isHoldingSomething;
    public bool isFood;

    void Start()
    {
        isFull = false;
        capacity = (maxInventorySlot + 10) * maxStack;
        PopulateSlotList();

        selectedSlotIndex = 0;
        if (hotBarSlotsUI.transform.GetChild(0).childCount != 0)
        {
            string itemPath = "On Hand Global/" + hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name;
            itemHoldGlobal = PhotonNetwork.Instantiate(itemPath, Resources.Load<GameObject>(itemPath).transform.position, Resources.Load<GameObject>(itemPath).transform.rotation, 0);
            itemHoldGlobal.GetComponent<PhotonView>().RPC("SetParentToSender", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);

            itemHold = Instantiate(Resources.Load<GameObject>("On Hand/" + hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name), handHold.transform);
        }
    }

    private void Update()
    {
        if (!inventoryStorage.activeSelf && (Input.GetAxis("Mouse ScrollWheel") != 0 || Int16.TryParse(Input.inputString, out Int16 n))) {
            selectedSlotIndex -= Input.GetAxis("Mouse ScrollWheel") * selectionScrollSpeed;
            if (selectedSlotIndex >= 10f)
            {
                selectedSlotIndex = 0f;
            }
            else if (selectedSlotIndex < 0)
            {
                selectedSlotIndex = 9.9999f;
            }
            switch (Input.inputString) {
                case "0":
                    selectedSlotIndex = 9;
                    break;
                case "1": case "2": case "3": case "4": case "5": case "6": case "7": case "8": case "9":
                    selectedSlotIndex = Int16.Parse(Input.inputString) - 1;
                    break;
                default:
                    break;
            }
            selectedSlotUI.transform.SetParent(selectSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)), false);

            updateEquip();
        }

        
        if (itemHold) {
            isHoldingSomething = true;
            handHold.GetComponent<WeaponAnimationController>().HandleCooldown();

            if (!handHold.GetComponent<WeaponAnimationController>().isSwinging && !handHold.GetComponent<WeaponAnimationController>().isOnCooldown && Input.GetKeyDown(GetComponentInParent<PlayerMovement2>().attackKey))
            {
                handHold.GetComponent<WeaponAnimationController>().SwingWeapon();
            }
            else
            {
                handHold.GetComponent<WeaponAnimationController>().HandleSway();
                handHold.GetComponent<WeaponAnimationController>().HandleBob();
            }
        }
        else{
            isHoldingSomething = false; 
        }
    }

    public void updateEquip() {
        if (!itemHold) {
            if (hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).childCount != 0)
            {
                string itemPath = "On Hand Global/" + hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name;
                itemHoldGlobal = PhotonNetwork.Instantiate(itemPath, Resources.Load<GameObject>(itemPath).transform.position, Resources.Load<GameObject>(itemPath).transform.rotation, 0);
                itemHoldGlobal.GetComponent<PhotonView>().RPC("SetParentToSender", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);

                itemHold = Instantiate(Resources.Load<GameObject>("On Hand/" + hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name), handHold.transform);
            }
        }
        else {
            Destroy(itemHold);
            PhotonNetwork.Destroy(itemHoldGlobal);
            if (handHoldGlobal.transform.childCount != 0f) PhotonNetwork.Destroy(handHoldGlobal.transform.GetChild(0).gameObject); 
            if (hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).childCount != 0)
            {
                string itemPath = "On Hand Global/" + hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name;
                itemHoldGlobal = PhotonNetwork.Instantiate(itemPath, Resources.Load<GameObject>(itemPath).transform.position, Resources.Load<GameObject>(itemPath).transform.rotation, 0);
                itemHoldGlobal.GetComponent<PhotonView>().RPC("SetParentToSender", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);

                itemHold = Instantiate(Resources.Load<GameObject>("On Hand/" + hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name), handHold.transform);
            }
        }
    }

    private void PopulateSlotList()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Item Slot");
        foreach (GameObject item in prefabs) {
            itemList.Add(new Item(item.name, 0));
        }    

        foreach (Transform child in inventorySlotsUI.transform)
        {
            slotList.Add(child.gameObject);
            if (child.transform.childCount == 1)
            {
                AddToItemList(child.transform.GetChild(0).gameObject.name, Int16.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text));
            }
        }

        foreach (Transform child in hotBarSlotsUI.transform)
        {
            slotList.Add(child.gameObject);
            if (child.transform.childCount == 1)
            {
                AddToItemList(child.transform.GetChild(0).gameObject.name, Int16.Parse(child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text));
            }
        }
    }

    public void AddToInventory(GameObject item)
    {
        if (CheckIfFull(item))
        {
            isFull = true;

        }
        else if (FindNextSlot(item) != null)
        {
            isFull = false;
            whatSlotToEquip = FindNextSlot(item);
            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>("Item Slot/" + item.GetComponent<InteractableObject>().GetItemName()), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation, whatSlotToEquip.transform);
            itemToAdd.name = item.GetComponent<InteractableObject>().GetItemName();
            itemToAdd.transform.localScale = new Vector3(1, 1, 1);

            AddToItemList(item.GetComponent<InteractableObject>().GetItemName(), Int16.Parse(itemToAdd.transform.GetChild(0).GetComponent<Text>().text));
        }
        else
        {
            isFull = false;
        }
    }

    private GameObject FindNextSlot(GameObject item)
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 1)
            {
                if (slot.transform.GetChild(0).gameObject.name == item.GetComponent<InteractableObject>().GetItemName() && Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) <= maxStack - 1)
                {
                    slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + 1).ToString();
                    AddToItemList(slot.transform.GetChild(0).gameObject.name, 1);
                    return null;
                }
            }
        }

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0) return slot;
        }
        return null;
    }

    public GameObject getSlot(string name, int count)
    {
        foreach (var slot in slotList)
        {
            if (slot.transform.childCount == 0) return slot;
            else {
                if (slot.transform.GetChild(0).gameObject.name == name && Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + count <= maxStack) {
                    return slot;
                }
            }
        }
        return null;
    }

    public bool CheckIfFull(GameObject item)
    {
        int counterSlot = 0;
        int counterItem = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0) return false;
            else if (slot.transform.childCount == 1)
            {
                if (slot.transform.GetChild(0).gameObject.name == item.GetComponent<InteractableObject>().GetItemName())
                {
                    counterItem += Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                }
                else counterSlot++;
            }
        }
        if ((counterSlot == maxInventorySlot + 10) || (counterItem == (maxInventorySlot + 10 - counterSlot) * maxStack)) return true;
        return false;
    }

    public bool checkInStringList(string itemName, List<string> list)
    {
        foreach (string item in list)
        {
            if (itemName == item) return true;
        }
        return false;
    }

    // add to list for faster craftable check
    public void AddToItemList(string name, int count) {
        countInventory += count;
        foreach (Item item in itemList) {
            if (item.name == name) {
                item.count += count;
                return;
            }
        }
    }

    // for check if you have enough ingredient
    public void RemoveFromItemList(string name, int count) {
        countInventory -= count;
        foreach (var item in itemList) {
            if (item.name == name) {
                item.count -= count;
                return;
            }
        }
    }

    // for remove ingredient after craft based on slot
    public void RemoveFromInventory(string name, int count) {
        RemoveFromItemList(name, count);
        foreach (var slot in slotList) {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == name) {
                if (Int16.Parse(slot.transform.GetChild(0).GetComponent<DragDrop>().count.text) < count) {
                    count -= Int16.Parse(slot.transform.GetChild(0).GetComponent<DragDrop>().count.text);
                    Destroy(slot.transform.GetChild(0).gameObject);
                }
                else if (Int16.Parse(slot.transform.GetChild(0).GetComponent<DragDrop>().count.text) == count){
                    Destroy(slot.transform.GetChild(0).gameObject);
                    return;
                }
                else {
                    slot.transform.GetChild(0).GetComponent<DragDrop>().count.text = (Int16.Parse(slot.transform.GetChild(0).GetComponent<DragDrop>().count.text) - count).ToString();
                    return;
                }
            }
        }
    }

    public bool enoughIngredientCount(string name, int count) {
        foreach (var item in itemList) {
            if (item.name == name){
                if (item.count >= count) return true;
                else return false;
            }
        }
        return false;
    }


}
