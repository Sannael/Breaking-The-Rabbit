using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItensActive : MonoBehaviour
{
    public Dictionary<int, InventorySlot> inventory = new Dictionary<int, InventorySlot>();

    private void FixedUpdate() 
    {
        inventory = CoreInventory._instance.inventory.inventory;
        foreach(var i in inventory)
        {
            if(i.Value.item.inventory == true && i.Value.item.used == false)
            {
                i.Value.item.ApplyUse();
            }
            if(i.Value.item.inventory == false && i.Value.item.used == true)
            {
                i.Value.item.DisUseItem();
            }
        }  
    }
}
