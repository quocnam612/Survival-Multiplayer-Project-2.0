using System;
using UnityEngine;
using UnityEngine.UI;

public class FuelSlot : MonoBehaviour
{
    public float burnMultiplier = 1.17f;
    public float burnTime;
    public float maxBurnTime;
    public float fireLoopSpeed;
    public InventorySystem inventory;
    public CookingSystem cookingSystem;
    public UnityEngine.UI.Image progressIndicator;
    public UnityEngine.UI.Image fireIcon;

    private void Update()
    {
        updateBurnTime();
        if (!cookingSystem.isCooking) {
            drainFuel();
        }

        if (transform.childCount == 1)
        {
            if (transform.GetChild(0).GetComponent<DragDrop>().isFuel) {
                fireIcon.enabled = true;
                burnTime += transform.GetChild(0).GetComponent<DragDrop>().energy * Int16.Parse(gameObject.transform.GetChild(0).GetComponent<DragDrop>().count.text);
                inventory.RemoveFromItemList(gameObject.transform.GetChild(0).gameObject.name, Int16.Parse(gameObject.transform.GetChild(0).GetComponent<DragDrop>().count.text));
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
            else {
                fireIcon.enabled = false;
            }
        }
        else {
            fireIcon.enabled = true;
        }
    }

    private void fireIconLoop(float multiplier) {
        if (fireIcon.fillAmount < 1f) {
            fireIcon.fillAmount += fireLoopSpeed * Time.deltaTime * multiplier;
        }
        else {
            fireIcon.fillAmount = 0f;
        }
    }

    private void updateBurnTime() {
        if (burnTime > 0f) {
            progressIndicator.fillAmount = burnTime / maxBurnTime;
        }
        else {
            fireIcon.fillAmount = 0f;
            progressIndicator.fillAmount = 0f;
        }
    }

    public void burnFuel() {
        burnTime -= Time.deltaTime * burnMultiplier;
        fireIconLoop(1f);
    }

    public void drainFuel() {
        burnTime -= Time.deltaTime / 6.9f;
        fireIconLoop(0.33f);
    }
}
