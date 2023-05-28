using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class teste : MonoBehaviour
{
    public Item item0;
    public Item[] bau;
    public InputActionReference hotbar0;
    public bool take = false;
    public bool candrop;
    void Update()
    {
        if(hotbar0.action.IsPressed() && candrop == true)
        {
            Bau();
            //rand();
            /*Debug.Log("FOI?");
            CoreInventory._instance.inventory.UpdateHotbarSlots(false, true);
            CoreInventory._instance.inventory.UpdateWeaponsSlots(false, true);
            CoreInventory._instance.inventory.UpdateInventorySlots(false, true);*/
        }
    }

    private void rand()
    {
        bool legend = false;
        while(legend == false)
        {
            int id = Random.Range(0, 851);
            if(id == 213|| id == 422|| id == 343 || id== 12)
            {
                legend = true;
                break;
            }
        }
        
    }

    private void Bau()
    {
        candrop = false;
        int drop = Random.Range(0, bau.Length);
        GameObject obj = Instantiate(bau[drop].thisPrefabDrop); 
        Vector3 v = this.transform.position;
        v[0] += 3;
        obj.transform.position = v;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(take == false)
        {
            take = true;
            CoreInventory._instance.inventory.GetItem(item0, item0.itemAmount, item0.unique, item0.pileable, item0.weapon);
            Destroy(this.gameObject);
        }
    }
}
