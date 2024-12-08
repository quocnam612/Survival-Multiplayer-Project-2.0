using UnityEngine;

public class SwapButtons : MonoBehaviour
{
    public GameObject overviewUI;
    public GameObject inventoryUI;
    public GameObject settingsUI;

    public void overviewUIOn() {
        overviewUI.SetActive(true);
        inventoryUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void inventoryUIOn() {
        overviewUI.SetActive(false);
        inventoryUI.SetActive(true);
        settingsUI.SetActive(false);
    }

    public void settingsUIOn() {
        overviewUI.SetActive(false);
        inventoryUI.SetActive(false);
        settingsUI.SetActive(true);
    }
}
