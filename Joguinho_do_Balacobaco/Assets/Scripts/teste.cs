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
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(take == false)
        {
            take = true;
            CoreInventory._instance.inventory.GetItem(item0, item0.itemAmount, item0.unique, item0.stakeable);
            Destroy(this.gameObject);
        }
        
    }
}
