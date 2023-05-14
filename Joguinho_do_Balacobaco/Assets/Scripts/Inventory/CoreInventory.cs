using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreInventory : MonoBehaviour
{
    public static CoreInventory _instance;
    public Inventory inventory;
    public InventoryDescs inventoryDescs;

    public void Awake() 
    {
        _instance = this;    
    }
}
