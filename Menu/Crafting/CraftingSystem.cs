using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [Serializable]
    public class Ingredient
    {
        public string name;
        public int count = 0;
        [HideInInspector] public GameObject textUI;
    }

    [Serializable]
    public class Recipe
    {
        public string name;
        public int count = 0;
        public string description;
        public List<Ingredient> ingredients;
        [HideInInspector] public GameObject recipeUI;
        public Sprite icon
        {
            get
            {
                return Resources.Load<Sprite>("Icons/" + name);
            }
        }
    }

    public InventorySystem inventorySystem
    {
        get
        {
            return gameObject.GetComponent<InventorySystem>();
        }
    }

    public List<Recipe> recipes;
    public GameObject craftMenu;
    public GameObject craftSlot;
    public GameObject recipeHolder;
    public GameObject recipeDisplayer;
    public Color32 craftableColor = new Color32(234, 234, 234, 170);
    public Color32 uncraftableColor = new Color32(170, 170, 170, 170);
    public Color32 notEnoughIngredientColor = new Color32(255, 50, 50, 220);
    public Color32 enoughIngredientColor = new Color32(255, 255, 255, 220);
    private bool toggle = true;

    private void Start()
    {
        foreach (var recipe in recipes) {
            recipe.recipeUI = (GameObject)Instantiate(Resources.Load<GameObject>("UI/Recipe"), recipeHolder.transform.position, recipeHolder.transform.rotation, recipeHolder.transform);
            GameObject ingredientHolder = recipe.recipeUI.transform.GetChild(2).gameObject;
            recipe.recipeUI.transform.GetChild(0).GetComponent<Image>().sprite = recipe.icon;
            recipe.recipeUI.transform.GetChild(1).GetComponent<Text>().text = recipe.name;

            RecipeController controller = recipe.recipeUI.GetComponent<RecipeController>();
            controller.craftingSystem = GetComponent<CraftingSystem>();
            controller.craftSlot = craftSlot.GetComponent<CraftSlot>();
            controller.recipeName = recipe.name;
            controller.displayer = recipeDisplayer;
            controller.description = recipe.description;
            controller.inventorySystem = inventorySystem;
            controller.count = recipe.count;

            foreach (var ingredient in recipe.ingredients) {
                ingredient.textUI = (GameObject)Instantiate(Resources.Load<GameObject>("UI/Ingredient"), ingredientHolder.transform.position, ingredientHolder.transform.rotation, ingredientHolder.transform);
                ingredient.textUI.GetComponent<Text>().text = "• " + ingredient.name + " ×" + ingredient.count;
                controller.ingredients.Add(new RecipeController.Ingredient(ingredient.name, ingredient.count));
            }
        }
    }

    private void Update() {
        if (craftMenu.activeSelf && toggle) {
            checkCraftabke();
            toggle = false;
        }
        else if (!craftMenu.activeSelf && !toggle) {
            toggle = true;
            if (craftSlot.gameObject.transform.childCount != 0)
            {
                GameObject returnSlot = inventorySystem.getSlot(craftSlot.transform.GetChild(0).gameObject.name, Int16.Parse(craftSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text));
                if (returnSlot.transform.childCount == 0)
                {   
                    craftSlot.gameObject.transform.GetChild(0).transform.SetParent(returnSlot.transform, false);
                }
                else {
                    returnSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (Int16.Parse(returnSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) + Int16.Parse(craftSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text)).ToString();
                    Destroy(craftSlot.transform.GetChild(0).gameObject);
                }
            }
        }
    }

    public void checkCraftabke() {
        foreach (var recipe in recipes)
        {
            bool craftable = true;
            foreach (var ingredient in recipe.ingredients)
            {
                if (!inventorySystem.enoughIngredientCount(ingredient.name, ingredient.count))
                {
                    craftable = false;
                    ingredient.textUI.GetComponent<Text>().color = notEnoughIngredientColor;
                }
                else
                {
                    ingredient.textUI.GetComponent<Text>().color = enoughIngredientColor;
                }
            }

            if (craftable)
            {
                recipe.recipeUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = craftableColor;
                recipe.recipeUI.transform.GetChild(4).gameObject.SetActive(true);
            }
            else
            {
                recipe.recipeUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = uncraftableColor;
                recipe.recipeUI.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
    }
}
