using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "ChestItens", menuName = "Scriptable/ChestItens")]
public class ChestItens : ScriptableObject 
{
    public bool isChecked;
    public Item[] allItens;
    [Header("Player Items & guns")]
    public Item[] playerHotbarItens = new Item[6];
    public Item[] playerInventoryItens = new Item[10];
    public Item[] playerWeapons = new Item[3];
    public Item ChestDrop(int dropType)
    {
        Item itemReturn = null;
        if(isChecked == false)
        {
            isChecked = true; //Isso aqui é se caso tiver travando busca pelos itens, fazer essa checagem pra buscar uma vez só, dsp vejo
            allItens = Resources.LoadAll("Drops", typeof(Item)).Cast<Item>().ToArray();
        }

        bool haveItem = false;
        //allItens = Resources.LoadAll("Drops", typeof(Item)).Cast<Item>().ToArray();
        int itemDrop = Random.Range(0, allItens.Length);

        if(allItens[itemDrop].weapon == dropType)
        {
            if(dropType == 0)
            {
                haveItem = DropItem(allItens[itemDrop]);
            }
            else if(dropType == 1 || dropType == 2)
            {
                haveItem = DropGunOrMelee(allItens[itemDrop]);
            }
        }
        else if(dropType == 3)
        {
            haveItem = (DropAll(allItens[itemDrop]));
        }

        if(haveItem == false)
        {
            itemReturn = allItens[itemDrop];
        }
        else
        {
            itemReturn = null;
        }
        return itemReturn;
    }

    public bool DropItem(Item item)
    {
        bool equals = false;

        foreach(var i in CoreInventory._instance.inventory.hotbar)
        {
            playerHotbarItens[i.Key] = i.Value.item;
        }
        for(int i =0; i < playerHotbarItens.Length; i ++)
        {
            if(item == playerHotbarItens[i])
            {
                equals = true;
            }
        }

        foreach(var i in CoreInventory._instance.inventory.inventory)
        {
            playerInventoryItens[i.Key] = i.Value.item;
        }
        for(int i =0; i < playerInventoryItens.Length; i ++)
        {
            if(item == playerInventoryItens[i])
            {
                equals = true;
            }
        }
        return equals;
    }

    public bool DropGunOrMelee(Item item)
    {
        bool equals = false;
        foreach(var i in CoreInventory._instance.inventory.weapons)
        {
            playerWeapons[i.Key] = i.Value.item;
        }
        for(int i =0; i < playerWeapons.Length; i ++)
        {
            if(item == playerWeapons[i])
            {
                equals = true;
            }
        }
        return equals;
    }

    public bool DropAll(Item item)
    {
        bool equals = false;

        foreach(var i in CoreInventory._instance.inventory.hotbar)
        {
            playerHotbarItens[i.Key] = i.Value.item;
        }
        for(int i =0; i < playerHotbarItens.Length; i ++)
        {
            if(item == playerHotbarItens[i])
            {
                equals = true;
            }
        }

        foreach(var i in CoreInventory._instance.inventory.inventory)
        {
            playerInventoryItens[i.Key] = i.Value.item;
        }
        for(int i =0; i < playerInventoryItens.Length; i ++)
        {
            if(item == playerInventoryItens[i])
            {
                equals = true;
            }
        }

        foreach(var i in CoreInventory._instance.inventory.weapons)
        {
            playerWeapons[i.Key] = i.Value.item;
        }
        for(int i = 0; i < playerWeapons.Length; i ++)
        {
            if(item == playerWeapons[i])
            {
                equals = true;
            }
        }
        return equals;
    }
}
