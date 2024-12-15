using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private WeaponAnimationController weaponController;
    private PlayerMovement2 playerMovement;
    
    private bool needToUpdateUI = true;
    private List<GameObject> objectPool = new List<GameObject>();

    private void Awake()
    {
        weaponController = handHold.GetComponent<WeaponAnimationController>();
        playerMovement = transform.root.GetComponent<PlayerMovement2>();
    }

    private void Update()
    {
        // Batch UI updates
        if (needToUpdateUI)
        {
            BatchUpdateInventoryUI();
            needToUpdateUI = false;
        }
    }

    private void BatchUpdateInventoryUI()
    {
        // Implement batching logic to update inventory UI
        foreach (var item in inventoryItems)
        {
            // Update UI elements for each item
        }
    }

    // Implement object pooling for dynamically created UI elements
    private GameObject GetPooledObject(string itemName)
    {
        // Check if the object pool contains an inactive object with the specified itemName
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy && obj.name == itemName)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If not found, create a new object and add it to the pool
        GameObject newObj = Instantiate(Resources.Load<GameObject>(itemName));
        newObj.name = itemName;
        objectPool.Add(newObj);
        return newObj;
    }

    private void ReturnPooledObject(GameObject obj)
    {
        // Deactivate the object and return it to the pool
        obj.SetActive(false);
    }
}
