using UnityEngine;
using UnityEngine.UI;

public class BagCapacityDisplayer : MonoBehaviour
{
    public InventorySystem inventorySystem;
    [HideInInspector] public Text capacityDisplayer {
        get {
            return GetComponent<Text>();
        }
    }

    void Update()
    {
        if (inventorySystem.countInventory >= inventorySystem.capacity * 85f / 100f)
        {
            capacityDisplayer.color = new Color32(255, 123, 123, 123);
        }
        else if (inventorySystem.countInventory >= inventorySystem.capacity * 65f / 100f)
        {
            capacityDisplayer.color = new Color32(255, 255, 123, 123);
        }
        else
        {
            capacityDisplayer.color = new Color32(255, 255, 255, 123);
        }

        capacityDisplayer.text = inventorySystem.countInventory + "/" + inventorySystem.capacity;
    }
}
