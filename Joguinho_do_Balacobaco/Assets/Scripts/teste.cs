using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class teste : MonoBehaviour
{
    public Item item0, item1, item2;
    public InputActionReference hotbar0, hotbar1, hotbar2;
    public bool take = false;
    void Update()
    {
        if(hotbar0.action.IsPressed())
        {
            Debug.Log("FOI?");
            CoreInventory._instance.inventory.UpdateHotbarSlots(false, true);
            CoreInventory._instance.inventory.UpdateWeaponsSlots(false, true);
            CoreInventory._instance.inventory.UpdateInventorySlots(false, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(take == false)
        {
            take = true;
            CoreInventory._instance.inventory.GetItem(item0, item0.itemAmount, item0.unique, item0.pileable);
            Destroy(this.gameObject);
        }
        
    }
}
