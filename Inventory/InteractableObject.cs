using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public string itemName;
    public bool pickable = true;
    public bool interactable = false;
    public bool stackable = true;

    public string GetItemName()
    {
        return itemName;  
    }
}