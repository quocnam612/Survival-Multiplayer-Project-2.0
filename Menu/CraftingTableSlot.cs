using System;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTableSlot : MonoBehaviour
{
    public float ingredientCraft;
    public Image icon;
    public Image progressBar;
    public InventorySystem inventorySystem;
    public CraftingSystem craftingSystem;

    public CraftingSystem.Recipe recipe;
    private bool craftable;
    private Dictionary<string, int> ingredientDictionary;

    private void Start() {
        ingredientDictionary = new Dictionary<string, int>();
        foreach (var ingredient in recipe.ingredients) {
            ingredientDictionary[ingredient.name] = ingredient.count;
        }
    }

    private void Update() {
        int playerIngredient = 0;

        foreach (var item in inventorySystem.itemList) {
            if (ingredientDictionary.ContainsKey(item.name)) {
                playerIngredient += Mathf.Min(item.count, ingredientDictionary[item.name]);
            }
        }

        progressBar.fillAmount = (float)playerIngredient / ingredientCraft;
        if (progressBar.fillAmount < 1f) {
            icon.color = new Color32(255, 255, 255, 69);
            craftable = false;  
        } else {
            icon.color = new Color32(255, 255, 255, 220);
            craftable = true;
        }
    }

    public void craftTable() {
        if (craftable && !inventorySystem.isFull) {
            GameObject slot = inventorySystem.getSlot(recipe.name, recipe.count);
            if (slot) {
                foreach (var ingredient in recipe.ingredients) {
                    inventorySystem.RemoveFromInventory(ingredient.name, ingredient.count);
                }
                if (slot.transform.childCount == 0) {
                    GameObject itemCrafted = Instantiate(Resources.Load("Item Slot/" + recipe.name), slot.transform.position, slot.transform.rotation, slot.transform) as GameObject;
                    itemCrafted.name = recipe.name;
                } else {
                    var itemCountText = slot.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    itemCountText.text = (int.Parse(itemCountText.text) + 1).ToString();
                }
            }
        }
    }
}
