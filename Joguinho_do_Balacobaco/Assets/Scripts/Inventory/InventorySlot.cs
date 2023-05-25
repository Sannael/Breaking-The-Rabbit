using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventorySave inventorySave;
    public SlotType slotType;
    public int idSlot;
    public Item item;
    public int itemId;
    public Image itemImage;
    public int itemAmount;
    public int itemPile;
    public bool unique, pileable, stakable;
    public string itemDesc;
    public TMP_Text itemCount;
    public bool itemSelected;
    public bool consumible, canMove, canDestroy;
    
    public void CreateSlot(int id, Item i)
    {
        idSlot = id;
        item = i;
    }
    public void UpdateSlot(bool insert, bool resetValues)
    {
        if(resetValues == true)
        {
            try{ResetItensInfo();}catch{}
            return;
        }
        if(item.isEmpty == true)
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            itemCount.enabled = false;
            itemAmount = 0;
            unique = item.unique;
            pileable = item.pileable;
            stakable = item.stakable;
            consumible = item.consumible;
            canMove = item.canMove;
            canDestroy = item.canDestroy;
        }
        else
        {
            itemId = item.itemId;
            itemImage.sprite = item.itemSprite;
            itemImage.enabled = true;
            unique = item.unique;
            pileable = item.pileable;
            stakable = item.stakable;
            consumible = item.consumible;
            canMove = item.canMove;
            canDestroy = item.canDestroy;
            if(unique == false)
            {
                itemCount.enabled = true;
                bool parse = int.TryParse(CoreInventory._instance.inventory.GetItemAmount(item), out itemAmount);
                itemCount.text = itemAmount.ToString();            
            }
            else
            {
                itemAmount = 0;
                itemCount.text = "";
            }
            if(itemAmount == 0)
            {
                itemCount.text = "";
            }
        }

        if(insert == true)
        {
            inventorySave.InsertItemInfos(idSlot, item.itemId, itemAmount, itemPile, item);
        }

    }
    public void ReUpdateSlot()
    {
        itemId = item.itemId;
        itemImage.sprite = item.itemSprite;
        itemImage.enabled = true;
        unique = item.unique;
        pileable = item.pileable;
        stakable = item.stakable;
        consumible = item.consumible;
        canMove = item.canMove;
        canDestroy = item.canDestroy;
        if(unique == false)
        {
            itemCount.enabled = true;
            itemCount.text = itemAmount.ToString();            
        }
        else
        {
            itemCount.text = "";
        }
        if(itemAmount == 0)
        {
            itemCount.text = "";
        }
        CoreInventory._instance.inventory.ReGetItem(item, itemAmount, unique, pileable);
    }
    public void ResetItensInfo()
    {
        int[] values = inventorySave.GiveItemInfos(idSlot);
        itemAmount = values[0];
        itemPile = values[1];
        item = inventorySave.TakeValues(idSlot);
        ReUpdateSlot();

        Debug.Log("Reset");
        Debug.Log("Slot_Id = " + idSlot +  "Item_Id = " + itemId + " / " + values[0] + " ItemAmount = " + itemAmount+ " / " +values[1]);
    }
    public void EnableButtons()
    {
        CoreInventory._instance.inventoryDescs.EnableButtons(consumible, canDestroy);

        CoreInventory._instance.inventoryDescs.item = item;
        CoreInventory._instance.inventoryDescs.idSlot = idSlot;
        CoreInventory._instance.inventoryDescs.slotType = slotType;

    }
    public void OnDrop(PointerEventData eventData)
    {
        
        CoreInventory._instance.inventory.OnDropDone(this, idSlot, slotType);
    }
}
