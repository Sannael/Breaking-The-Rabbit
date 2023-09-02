using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot", menuName = "Scriptable/Slot", order = 1)]
public class InventorySave : ScriptableObject
{
    public int slotAmount; //Quantidade de slots
    public int[] itemId; //Id de cada item
    public int[] itemAmount; //Quantidade que o player tem de cada item
    public int[] itemStak; //Quantidade de stacks que o player tem de cada item
    public Item[] item; //o item em si
    
    public void InsertItemInfos(int id, int idItem, int amount, int stak, Item i) 
    { //Salva as informaçãos dos itens em suas posições 
        itemId[id] = idItem;
        itemAmount[id] = amount;
        itemStak[id] = stak;
        item[id] = i;
        if(id == (slotAmount -1))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().exitTriggers ++;
        }
    }

    public int[] GiveItemInfos(int id)
    { //Devolve as informações do item
        int[] ret = new int[2];
        ret[0] = itemAmount[id];
        ret[1] = itemStak[id];
        return ret;
    } 

    public Item TakeValues(int id)
    { //Devolve o item de acordo com a posição
        if(item[id] == null)
        {
            item[id] = CoreInventory._instance.inventory.itemEmpty; //Busca o item vazio
        }
        return item[id];
    }

    public void ResetSlot(int id)
    { //Reseta as informações para as informações iniciais de cada slot
        itemId[id] = 0;
        itemAmount[id] = 0;
        itemStak[id] = 0;
        item[id] = CoreInventory._instance.inventory.itemEmpty; //Busca o item vazio
    }

}
