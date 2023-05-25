using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot", menuName = "Scriptable/Slot", order = 1)]
public class InventorySave : ScriptableObject
{
    public int slotAmount;
    public int[] itemId;
    public int[] itemAmount;
    public int[] itemPile;
    public Item[] item;

    void Awake()
    {
    }
    public void InsertItemInfos(int id, int idItem, int amount, int pile, Item i)
    {
        itemId[id] = idItem;
        itemAmount[id] = amount;
        itemPile[id] = pile;
        item[id] = i;
    }

    public int[] GiveItemInfos(int id)
    {
        int[] ret = new int[2];
        ret[0] = itemAmount[id];
        ret[1] = itemPile[id];
        return ret;
    } 

    public Item TakeValues(int id)
    {
        if(item[id] == null)
        {
            item[id] = CoreInventory._instance.inventory.itemEmpty;
        }
        return item[id];
    }

    public void ResetSlot(int id)
    {
        itemId[id] = 0;
        itemAmount[id] = 0;
        itemPile[id] = 0;
    }

}
