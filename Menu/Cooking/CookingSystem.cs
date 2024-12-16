using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookingSystem : MonoBehaviour 
{
    private InventorySystem _inventorySystem;
    private DragDrop cookSlotDragDrop;
    private Text cookedSlotCountText;
    
    private void Awake()
    {
        _inventorySystem = GetComponent<InventorySystem>();
        if (cookSlot) cookSlotDragDrop = cookSlot.GetComponent<DragDrop>();
        if (cookedSlot && cookedSlot.transform.childCount > 0)
            cookedSlotCountText = cookedSlot.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }
    
    private void UpdateCooking()
    {
        if (!CanCook()) 
        {
            ResetCooking();
            return;
        }
        
        if (countDown > 0f)
        {
            countDown -= Time.deltaTime;
            fuelSlot.burnFuel();
            progressInicator.fillAmount = 1 - countDown / cookTime;
        }
        else
        {
            CompleteCooking();
        }
    }
    
    private bool CanCook()
    {
        return fuelSlot.burnTime > 0f && 
               cookSlotDragDrop != null && 
               cookSlotDragDrop.cookable &&
               (!cookedSlotCountText || int.Parse(cookedSlotCountText.text) < _inventorySystem.maxStack);
    }
}
