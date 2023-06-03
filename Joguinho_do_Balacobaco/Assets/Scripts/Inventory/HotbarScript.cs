using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarScript : MonoBehaviour
{    //Vou usar isso pra controlar os itens que tem na hotbar (ACHO) criei mas deixei pra mexer dps
    public int[] hotbarSlots = new int[6];
    public Dictionary<int, InventorySlot> hotbar = new Dictionary<int, InventorySlot>();

    private void FixedUpdate() 
    {
        hotbar = CoreInventory._instance.inventory.hotbar;
        foreach(var i in hotbar)
        {
            if(i.Value.item.hotbar == true)
            {
                i.Value.item.ApplyUse();
            }
        }        
    } 
}
