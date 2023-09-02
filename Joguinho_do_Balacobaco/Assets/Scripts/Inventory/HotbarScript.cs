using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HotbarScript : MonoBehaviour
{   //Vou usar isso pra controlar os itens que tem na hotbar (ACHO) criei mas deixei pra mexer dps
    public int[] hotbarSlots = new int[6]; 
    public Dictionary<int, InventorySlot> hotbar = new Dictionary<int, InventorySlot>();
    public AudioClip inventoryFullSound;
    public InputActionReference[] hotbarInput = new InputActionReference[6];
    private PlayerScript ps;
    public int tryConsume;
    private bool consuming;
    private void Start() 
    {
        consuming = false;
        tryConsume = 10;
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        for(int i =0; i < hotbarInput.Length; i ++)
        {
            hotbarInput[i] = ps.hotbar[i];
        }
    } 
    private void Update() 
    {
        hotbar = CoreInventory._instance.inventory.hotbar;
        foreach(var i in hotbar)
        {
            if(i.Value.item.hotbar == true && i.Value.item.used == false)
            {
                i.Value.item.ApplyUse();
            }
        } 
        if(hotbarInput[0].action.IsPressed() && tryConsume !=0 && consuming == false)
        {
            tryConsume = 0;
            Consume(0);
            consuming = true;
        }
        if(hotbarInput[1].action.IsPressed() && tryConsume !=1 && consuming == false)
        {
            tryConsume = 1;
            Consume(1);
            consuming = true;
        }
        if(hotbarInput[2].action.IsPressed() && tryConsume !=2 && consuming == false)
        {
            tryConsume = 2;
            Consume(2);
            consuming = true;
        }
        if(hotbarInput[3].action.IsPressed() && tryConsume !=3 && consuming == false)
        {
            tryConsume = 3;
            Consume(3);
            consuming = true;
        }
        if(hotbarInput[4].action.IsPressed() && tryConsume !=4 && consuming == false)
        {
            tryConsume = 4;
            Consume(4);
            consuming = true;
        }
        if(hotbarInput[5].action.IsPressed() && tryConsume !=5 && consuming == false)
        {
            tryConsume = 5;
            Consume(5);
            consuming = true;
        }


        if(hotbarInput[0].action.IsPressed() == false && tryConsume == 0)
        {
            tryConsume = 10;
            consuming = false;
        }
        if(hotbarInput[1].action.IsPressed() == false && tryConsume == 1)
        {
            tryConsume = 10;
            consuming = false;
        }
        if(hotbarInput[2].action.IsPressed() == false && tryConsume == 2)
        {
            tryConsume = 10;
            consuming = false;
        }
        if(hotbarInput[3].action.IsPressed() == false && tryConsume == 3)
        {
            tryConsume = 10;
            consuming = false;
        }
        if(hotbarInput[4].action.IsPressed() == false && tryConsume == 4)
        {
            tryConsume = 10;
            consuming = false;
        }
        if(hotbarInput[5].action.IsPressed() == false && tryConsume == 5)
        {
            tryConsume = 10;
            consuming = false;
        }
    } 

    public void Consume(int hotbarSlot)
    {
        bool consumed = false;
        if(hotbar[hotbarSlot].consumible)
        {
            consumed = hotbar[hotbarSlot].item.ApplyConsume();
        }

        if(consumed)
        {
            Item a = hotbar[hotbarSlot].item;
            CoreInventory._instance.inventory.itemAmount[a] --;
            CoreInventory._instance.inventory.hotbar[hotbarSlot].itemAmount --;
            if(CoreInventory._instance.inventory.hotbar[hotbarSlot].itemAmount <1)
            {
                CoreInventory._instance.inventory.DestroyItem(hotbarSlot, SlotType.HOTBAR);
            }
            CoreInventory._instance.inventory.UpdateHotbarSlots(false);
        }
    }

    public void SetInventoryFullSound()
    {
        GameSounds.instance.CreateNewSound(inventoryFullSound);
    }
}
