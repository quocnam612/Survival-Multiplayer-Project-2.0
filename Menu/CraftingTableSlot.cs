using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CraftingTableSlot : MonoBehaviour
{
    public float ingredientCraft;
    public UnityEngine.UI.Image icon;
    public UnityEngine.UI.Image progressBar;
    public InventorySystem inventorySystem;
    public CraftingSystem craftingSystem;

    public CraftingSystem.Recipe recipe;
    private bool craftable;
    public int playerIngredient;

    private void Update() {
        playerIngredient = 0;

        foreach (var ingredient in recipe.ingredients) 
        {
            foreach (var item in inventorySystem.itemList)
            {
                if (item.name == ingredient.name)
                {
                    playerIngredient += Mathf.Min(item.count, ingredient.count);
                }
            }
        }

        progressBar.fillAmount = playerIngredient / ingredientCraft;
        if (progressBar.fillAmount < 1f)
        {
            icon.color = new Color32(255, 255, 255, 69);
            craftable = false;  
        }
        else
        {
            icon.color = new Color32(255, 255, 255, 220);
            craftable = true;
        }
    }

    public void craftTable() {
        if (craftable && !inventorySystem.isFull) {
            GameObject slot = inventorySystem.getSlot(recipe.name, recipe.count);
            if (slot) {
                foreach (var ingredient in recipe.ingredients)
                {
                    inventorySystem.RemoveFromInventory(ingredient.name, ingredient.count);
                }
                if (slot.transform.childCount == 0) {
                    GameObject itemCrafted = (GameObject)Instantiate(Resources.Load("Item Slot/" + recipe.name), slot.transform.position, slot.transform.rotation, slot.transform);
                    itemCrafted.name = recipe.name;
                }
                else {
                    slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + 1).ToString();
                }
            }
        }
    }
}
