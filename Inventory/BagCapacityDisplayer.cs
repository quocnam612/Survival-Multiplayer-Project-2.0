using UnityEngine;
using UnityEngine.UI;

public class BagCapacityDisplayer : MonoBehaviour
{
    public InventorySystem inventorySystem;
    [HideInInspector] public Text capacityDisplayer;
    Color maxInventoryColor = new Color(255, 0, 0, 200);

    void Start()
    {
         capacityDisplayer = GetComponent<Text>();
    }

    void Update()
    {
        capacityDisplayer.text = inventorySystem.countInventory + "/" + inventorySystem.capacity;

        if (inventorySystem.countInventory >= inventorySystem.capacity * 90f / 100f) {
            capacityDisplayer.color = maxInventoryColor;
        }
    }
}
