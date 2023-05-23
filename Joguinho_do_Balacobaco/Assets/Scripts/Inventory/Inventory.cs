using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum SlotType
{
    INVENTORY, HOTBAR, WEAPON
}
public class Inventory : MonoBehaviour
{
    public Item itemEmpty;
    [Header("Inventory Itens")]
    public RectTransform itensPanel;
    public GameObject inventorySlot;
    public int inventoryAmount = 10;

    [Header("Inventory Weapons")]
    public RectTransform weaponsPanel;
    public GameObject weaponSlot;
    public int weaponsAmount = 3;

    [Header("Inventory Hotbar")]
    public RectTransform hotbarPanel;
    public RectTransform playerHotbar;
    public GameObject hotbarSlot;
    public int hotbarAmount = 6;

    public Dictionary<int, InventorySlot> inventory = new Dictionary<int, InventorySlot>();
    public Dictionary<Item, int> itemAmount = new Dictionary<Item, int>();
    public Dictionary<int, InventorySlot> hotbar = new Dictionary<int, InventorySlot>();
    public Dictionary<int, InventorySlot> hotbarPlayer = new Dictionary<int, InventorySlot>();
    public Dictionary<int, InventorySlot> weapons = new Dictionary<int, InventorySlot>();
    private bool isInventoryCreate;

    [Header("Drag & Drop")]
    public Image itemDrag;

    [HideInInspector]public InventorySlot slotDrag;
    [HideInInspector] public InventorySlot slotDrop;

    [Header("Item Description")]
    public InventoryDescs inventoryDesc;
    public void Awake()
    {
        if(isInventoryCreate == false)
        {
            CreateItensSlot();
        }
    }

    public void CreateItensSlot()
    {
        for(int i =0; i < inventoryAmount; i ++)
        {
            GameObject s = Instantiate(inventorySlot, itensPanel);
            InventorySlot slot = s.GetComponent<InventorySlot>();
            slot.CreateSlot(i, itemEmpty);
            inventory.Add(i, slot);
        }

        for(int i =0; i < weaponsAmount; i ++)
        {
            GameObject s = Instantiate(weaponSlot, weaponsPanel);
            InventorySlot slot = s.GetComponent<InventorySlot>();
            slot.CreateSlot(i, itemEmpty);
            weapons.Add(i, slot);
        }

        for(int i =0; i < hotbarAmount; i ++)
        {
            GameObject s = Instantiate(hotbarSlot, hotbarPanel);
            InventorySlot slot = s.GetComponent<InventorySlot>();
            slot.CreateSlot(i, itemEmpty);
            hotbar.Add(i, slot);

            GameObject s2 = Instantiate(hotbarSlot, playerHotbar);
            InventorySlot slot2 = s2.GetComponent<InventorySlot>();
            slot2.CreateSlot(i, itemEmpty);
            hotbarPlayer.Add(i, slot2);
        }
        UpdateInventorySlots();
        UpdateHotbarSlots();
        UpdateWeaponsSlots();
        UpdateHotbarPlayer();
        isInventoryCreate = true; 
    }

    public bool HotbarFull()
    {
        bool isFull = true;
        int busy = hotbar.Values.Count(x => x.item != itemEmpty);
        {
            if(busy < hotbarAmount)
            {
                isFull = false;
            }
            else
            {
                print("Hotbar Full");
            }
        }
        return isFull;
    }
    public bool InventoryFull()
    {
        bool isFull = true;
        int busy = inventory.Values.Count(x => x.item != itemEmpty);
        if(busy < inventoryAmount)
        {
            isFull = false;
        }
        else
        {
            print("Inventory Full");
        }
        return isFull;
    }

    public void GetItem(Item item, int amount, bool unique, bool stakeable)
    {
        if(itemAmount.ContainsKey(item) && stakeable == true && unique == false)
        {
            if(itemAmount[item] < item.maxItens)
            {
                itemAmount[item] += amount;
                UpdateHotbarSlots();
                UpdateInventorySlots();
            }
            else
            {
                //vo pensar dps
            }
        }

        else if(itemAmount.ContainsKey(item) && unique == true && stakeable == false)
        {
            //Meia noite eu te conto
        }
        else
        {
            if(HotbarFull() == false)
            {
                int i = hotbar.First(x => x.Value.item == itemEmpty).Key;
                hotbar[i].item = item;
                if(stakeable == true && unique == false)
                {
                    itemAmount.Add(item, amount);
                }
                else if(unique == true && stakeable == false)
                {
                    itemAmount.Add(item, amount);
                }
                UpdateHotbarSlots();
            }
            else if(InventoryFull() == false)
            {
                int i = inventory.First(x => x.Value.item == itemEmpty).Key;
                inventory[i].item = item;
                if(stakeable == true && unique == false)
                {
                    itemAmount.Add(item, amount);
                }
                UpdateInventorySlots();
            }
        }
    }
    public string GetItemAmount(Item item)
    {
        if(itemAmount.ContainsKey(item))
        {
            return itemAmount[item].ToString();
        }
        else
        {
            return "";
        }
    }

    public void UpdateInventorySlots()
    {
        foreach(KeyValuePair<int, InventorySlot> slot in inventory)
        {
            slot.Value.UpdateSlot();
        }
    }

    public void UpdateHotbarSlots()
    {
        foreach(var slot in hotbar)
        {
            slot.Value.UpdateSlot();
        }
        UpdateHotbarPlayer();
    }
    public void UpdateHotbarPlayer()
    {
        for(int i =0; i <hotbarAmount; i ++)
        {
            hotbarPlayer[i].item = hotbar[i].item;
        }    
        foreach(var slot2 in hotbarPlayer)
        {
            slot2.Value.UpdateSlot();
        }

    }
    public void UpdateWeaponsSlots()
    {
        foreach(var slot in weapons)
        {
            slot.Value.UpdateSlot();
        }
    }

    public void OnDropDone(InventorySlot drop, int idSlot, SlotType stype)
    {
        slotDrop = drop;

        try
        {

        Item iDrag = slotDrag.item;
        Item iDrop = slotDrop.item;

        if(slotDrop.slotType != SlotType.WEAPON && iDrag != null && iDrag.canMove == true && iDrop.canMove == true)
        {
            slotDrag.item = iDrop;
            slotDrop.item = iDrag;
            UpdateInventorySlots();
            UpdateHotbarSlots();
            SelectItem(idSlot, stype);
        }
        }
        catch{}
        slotDrag = null;
        slotDrop = null;
    }

    public void ChangeDescPanel(Item item, int language)
    {
        inventoryDesc.ChangeDesc(item.itemName[language], item.itemDesc[language], item.itemSprite);
    }

    public void DestroyItem(int idSlot, SlotType stype)
    {
        if(stype == SlotType.HOTBAR)
        {
            hotbar[idSlot].item = itemEmpty;
            UpdateHotbarSlots();
        }
        if(stype == SlotType.INVENTORY)
        {
            inventory[idSlot].item = itemEmpty;
            UpdateInventorySlots();
        }
        ChangeDescPanel(itemEmpty, 0);
    }

    public void SelectItem(int slotId, SlotType sType) 
    {
        if(sType == SlotType.HOTBAR)
        {
            if(hotbar[slotId].item == itemEmpty)
            {
                return;
            }
            for(int i = 0; i < hotbarAmount; i ++)
            {
                if(i == slotId)
                {
                    print(i);
                    hotbar[i].itemSelected = true;
                    hotbar[i].EnableButtons();
                }
                else
                {
                    hotbar[i].itemSelected = false;
                }
            }
            for(int i =0; i < inventoryAmount; i ++)
            {
                inventory[0].itemSelected = false;
            }
        }
        else if(sType == SlotType.INVENTORY)
        {
            for(int i = 0; i < inventoryAmount; i ++)
            {
                if(i == slotId)
                {
                    inventory[i].itemSelected = true;
                    inventory[i].EnableButtons();
                }
                else
                {
                    inventory[i].itemSelected = false;
                }
            }
            for(int i = 0; i < hotbarAmount; i ++)
            {
                hotbar[i].itemSelected = false;
            }
        }
        else
        {
            for(int i =0; i < inventoryAmount; i ++)
            {
                inventory[i].itemSelected = false;
            }
            for(int i =0; i < hotbarAmount; i ++)
            {
                hotbar[i].itemSelected = false;
            }
            CoreInventory._instance.inventoryDescs.DisableAllButtons();
        }
        UpdateWeaponsSlots();
    }
}
