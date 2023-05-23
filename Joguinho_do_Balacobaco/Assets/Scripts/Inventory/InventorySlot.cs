using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public SlotType slotType;
    public int idSlot;
    public Item item;
    public Image itemImage;
    public int itemAmount;
    public bool unique, stakeable;
    public string itemDesc;
    public TMP_Text itemCount;
    public bool itemSelected;
    public bool consumible, canMove, canDestroy;
    
    public void CreateSlot(int id, Item i)
    {
        idSlot = id;
        item = i;
    }
    public void UpdateSlot()
    {
        if(item.isEmpty == true)
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            itemCount.enabled = false;
            itemAmount = 0;
            unique = item.unique;
            stakeable = item.stakeable;
            consumible = item.consumible;
            canMove = item.canMove;
            canDestroy = item.canDestroy;
        }
        else
        {
            itemImage.sprite = item.itemSprite;
            itemImage.enabled = true;
            unique = item.unique;
            stakeable = item.stakeable;
            consumible = item.consumible;
            canMove = item.canMove;
            canDestroy = item.canDestroy;
            if(unique == false)
            {
                itemCount.enabled = true;
                itemCount.text = CoreInventory._instance.inventory.GetItemAmount(item);
            }
            else
            {
                itemCount.text = "";
            }
        }
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
