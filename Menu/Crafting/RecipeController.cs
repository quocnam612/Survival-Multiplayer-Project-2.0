using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeController : MonoBehaviour
{
    [Serializable]
    public class Ingredient
    {
        public string name;
        public int count;
        public Ingredient (string _name, int _count) {
            name = _name;
            count = _count;
        }
    }

    public CraftSlot craftSlot;
    public GameObject displayer;
    public string description;
    public string recipeName;
    public int count;
    public InventorySystem inventorySystem;
    public CraftingSystem craftingSystem;
    public List<Ingredient> ingredients;

    public void onClick()
    {
        GameObject slot = inventorySystem.getSlot(recipeName, count);

        if (slot) {
            GameObject itemCrafted = (GameObject)Instantiate(Resources.Load("Item Slot/" + recipeName), displayer.transform.GetChild(1).transform.position, displayer.transform.GetChild(1).transform.rotation, displayer.transform.GetChild(1).transform);
            displayer.transform.GetChild(0).GetComponent<Text>().text = "Crafted " + recipeName;


            foreach (var ingredient in ingredients)
            {
                inventorySystem.RemoveFromInventory(ingredient.name, ingredient.count);
                itemCrafted.name = recipeName;
                itemCrafted.transform.GetChild(0).GetComponent<Text>().text = count.ToString();
            }

            if (craftSlot.transform.childCount == 2) {
                GameObject slotTmp = inventorySystem.getSlot(craftSlot.transform.GetChild(0).gameObject.name, Int16.Parse(craftSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text));
                craftSlot.transform.GetChild(0).gameObject.transform.SetParent(slotTmp.transform, false);
                if (slotTmp.transform.childCount == 2) {
                    slotTmp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(slotTmp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + Int16.Parse(slotTmp.transform.GetChild(1).GetChild(0).GetComponent<Text>().text)).ToString();
                    Destroy(slotTmp.transform.GetChild(1).gameObject);
                }
            }
            inventorySystem.AddToItemList(recipeName, count);
            craftingSystem.checkCraftabke();
        }
        else {
            displayer.transform.GetChild(0).GetComponent<Text>().text = "Your inventory is full!";
        }
    }
}
