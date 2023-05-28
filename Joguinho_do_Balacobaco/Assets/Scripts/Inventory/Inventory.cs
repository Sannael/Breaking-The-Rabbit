using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;

public enum SlotType
{
    INVENTORY, HOTBAR, WEAPON, PLAYERHOTBAR //Salva os posiveis tipos de Slot
}
public class Inventory : MonoBehaviour
{
    public Item itemEmpty; //Armazena o item vazio
    [Header("Inventory Itens")]
    public RectTransform itensPanel; //Painel pra posicionar os slots
    public GameObject inventorySlot; //Prefab dos slots
    public int inventoryAmount = 10; //Quantidade de slots

    [Header("Inventory Weapons")]
    public RectTransform weaponsPanel; //Painel pra posicionar os slots
    public GameObject weaponSlot; //Prefab dos slots
    public int weaponsAmount = 3; //Quantidade de slots

    [Header("Inventory Hotbar")]
    public RectTransform hotbarPanel; //Painel pra posicionar os slots (Hotbar no Inventario)
    public RectTransform playerHotbar; //Painel pra posicionar os slots (Hotbar do Player)
    public GameObject hotbarSlot; //Prefab dos slots
    public GameObject playerHotbarSlot; //Prefab dos slots
    public int hotbarAmount = 6; //Quantidade de slots

    public Dictionary<int, InventorySlot> inventory = new Dictionary<int, InventorySlot>(); //inventario que armazena os itens dos slots do inventario em si
    public Dictionary<Item, int> itemAmount = new Dictionary<Item, int>();  //Armazena a quantidade que tem de cada item que tiver no inventario
    public Dictionary<int, InventorySlot> hotbar = new Dictionary<int, InventorySlot>(); //inventario que armazena os itens dos slots da hotbar do inventario
    public Dictionary<int, InventorySlot> hotbarPlayer = new Dictionary<int, InventorySlot>(); //inventario que armazena os itens dos slots da hotbar do player
    public Dictionary<int, InventorySlot> weapons = new Dictionary<int, InventorySlot>(); //inventario que armazena os itens dos slots das armas
    private bool isInventoryCreate; //Checa se o invetario foi criado

    [Header("Drag & Drop")]
    public Image itemDrag; //Item sendo movido
    [HideInInspector]public InventorySlot slotDrag; //Antigo slot do item movido
    [HideInInspector] public InventorySlot slotDrop; //Novo slot do item movido

    [Header("Item Description")]
    public InventoryDescs inventoryDesc;

    public InputActionReference t;
    public Item[] a;
    public void Awake()
    {
        if(isInventoryCreate == false)
        {
            CreateItensSlot(); //Cria o inventario
        }
    }
    void Update()
    {
        if(t.action.IsPressed())
        {
            for(int i = 0; i <a.Length; i ++)
            {
                if(itemAmount.ContainsKey(a[i]))
                {
                    Debug.Log(i + " = " + a[i]);
                }
                
            }
            Debug.Log(itemAmount.Count);
        }
    }
    public void ReTakeItensInfo()
    {
        foreach(KeyValuePair<int, InventorySlot> slot in inventory) //Pega posição por posição do dicionario
        {
            slot.Value.ResetItensInfo(); //Atualiza as posições
        }
        foreach(var slot in hotbar) //Mesma coisa do de cima soq de um jeitinho diferente, pra ficar anotado
        {
            slot.Value.ResetItensInfo();
        }
        foreach(var slot in weapons)
        {
            slot.Value.ResetItensInfo();
        }
        UpdateHotbarPlayer();
    }

    public void SaveItens()
    {
        UpdateHotbarSlots(true);
        UpdateInventorySlots(true);
        UpdateInventorySlots(true);
    }

    public void CreateItensSlot()
    {
        for(int i =0; i < inventoryAmount; i ++) //Pega a quantidade de slots
        {
            GameObject s = Instantiate(inventorySlot, itensPanel); //Cria slot por slot
            InventorySlot slot = s.GetComponent<InventorySlot>(); //Armazena o script dentro de cada slot
            slot.CreateSlot(i, itemEmpty); //Armazena os dados de cada slot (id, item inicial = vazio) 
            inventory.Add(i, slot); //Adiciona cada slot no seu dicionario
        }

        for(int i =0; i < weaponsAmount; i ++) //Pega a quantidade de slots
        {
            GameObject s = Instantiate(weaponSlot, weaponsPanel); //Cria slot por slot
            InventorySlot slot = s.GetComponent<InventorySlot>(); //Armazena o script dentro de cada slot
            slot.CreateSlot(i, itemEmpty); //Armazena os dados de cada slot (id, item inicial = vazio) 
            weapons.Add(i, slot); //Adiciona cada slot no seu dicionario
        }

        for(int i =0; i < hotbarAmount; i ++) //Pega a quantidade de slots
        {
            GameObject s = Instantiate(hotbarSlot, hotbarPanel); //Cria slot por slot
            InventorySlot slot = s.GetComponent<InventorySlot>(); //Armazena o script dentro de cada slot
            slot.CreateSlot(i, itemEmpty); //Armazena os dados de cada slot (id, item inicial = vazio) 
            hotbar.Add(i, slot); //Adiciona cada slot no seu dicionario

            GameObject s2 = Instantiate(playerHotbarSlot, playerHotbar); //Cria slot por slot
            InventorySlot slot2 = s2.GetComponent<InventorySlot>(); //Armazena o script dentro de cada slot
            slot2.CreateSlot(i, itemEmpty); //Armazena os dados de cada slot (id, item inicial = vazio) 
            hotbarPlayer.Add(i, slot2); //Adiciona cada slot no seu dicionario
        }
        UpdateInventorySlots(false); //Atualiza o inventario
        UpdateHotbarSlots(false); //Atualiza a hotbar do inventario
        UpdateWeaponsSlots(false); //Atualiza a aba de armas
        UpdateHotbarPlayer(); //Atualiza o hotbar do player
        isInventoryCreate = true; 
    }

    public bool HotbarFull() //Checa se a hotbar ta cheia
    {
        bool isFull = true;
        int busy = hotbar.Values.Count(x => x.item != itemEmpty); //Procura por itens não-vazios
        if(busy < hotbarAmount) //Se nem todos os espaços tiverem ocupados, a hotbar n ta cheia
        {
            isFull = false;
        }
        else
        {
            print("Hotbar Full");
        }
        
        return isFull;
    }
    public bool InventoryFull()
    {
        bool isFull = true;
        int busy = inventory.Values.Count(x => x.item != itemEmpty); //Procura por itens não-vazios
        if(busy < inventoryAmount) //Se nem todos os espaços tiverem ocupados, o inventario n ta cheio
        {
            isFull = false;
        }
        else
        {
            print("Inventory Full");
        }
        return isFull;
    }

    public bool GetItem(Item item, int amount, bool unique, bool pileable, int weapon) //Armazena o item que o player pegou
    {
        bool destroy = false;
        if(weapon != 0)
        {
            weapons[weapon-1].item = item;
            UpdateWeaponsSlots(false);
            return destroy;
        }
        if(itemAmount.ContainsKey(item) && pileable == true && unique == false) //checa se o player tem o item e se pode ser enpilhado
        {
            if(itemAmount[item] < item.maxItens) //Se n tiver atingido o máximo
            {
                itemAmount[item] += amount; //Adiciona a quantidade que tiver do item ao dicionario de quantidades
                UpdateHotbarSlots(false);
                UpdateInventorySlots(false);
                destroy = true;
            }
            else
            {
                destroy = false;
                //vo pensar dps
            }
        }

        else if(itemAmount.ContainsKey(item) && unique == true && pileable == false) //Se tiver o item e ele n poder ser empilhado
        {
            destroy = false;
            //Meia noite eu te conto
        }
        else //Se n tiver o item, ou o item for semi-unico
        {
            if(HotbarFull() == false) //Se a hotbar n tiver cheia
            {
                int i = hotbar.First(x => x.Value.item == itemEmpty).Key; //Procura o primeiro slot vazio ma hotbar e retorna seu Id
                hotbar[i].item = item; //o item do slot agr é o novo item
                if(pileable == true && unique == false)
                {
                    itemAmount.Add(item, amount); //Adiciona uma quantidade do item no dicionario de quantidade de item
                }
                else if(unique == true && pileable == false)
                {
                    itemAmount.Add(item, amount); //Adiciona uma quantidade do item no dicionario de quantidade de item
                }
                UpdateHotbarSlots(false);
                destroy = true;
            }
            else if(InventoryFull() == false)
            {
                int i = inventory.First(x => x.Value.item == itemEmpty).Key; //Procura o primeiro slot vazio no inventario e retorna seu Id
                inventory[i].item = item; //o item do slot agr é o novo item
                if(pileable == true && unique == false)
                {
                    itemAmount.Add(item, amount); //Adiciona uma quantidade do item no dicionario de quantidade de item
                }
                else if(unique == true && pileable == false)
                {
                    itemAmount.Add(item, amount); //Adiciona uma quantidade do item no dicionario de quantidade de item
                }
                UpdateInventorySlots(false);
                destroy = true;
            }
        }
        return destroy;
    }

    public void ReGetItem(SlotType stype, int idSlot, Item item, int amount, bool unique, bool pileable, int weapon) //mesma coisa da de cima, porem sem retornar e sem atualizar nem a hotbar, nem inventario
    {
        if(stype == SlotType.HOTBAR)
        {
            hotbar[idSlot].item = item;
            if(unique == true && pileable == true)
            {
                return;
            }
            else
            {
                itemAmount.Add(item, amount);
            }
        }
        else if(stype == SlotType.INVENTORY)
        {
            inventory[idSlot].item = item;
            if(unique == true && pileable == true)
            {
                return;
            }
            else
            {
                itemAmount.Add(item, amount);
            }
        }
        if(stype == SlotType.WEAPON)
        {
            weapons[idSlot].item = item;
        }
    }
    public string GetItemAmount(Item item) //Puxo a quantidade que tem do item 
    {
        if(itemAmount.ContainsKey(item)) //checa se tem o item no dicionario de quantidade
        {
            return itemAmount[item].ToString();
        }
        else
        {
            return "";
        }
    }

    public void UpdateInventorySlots(bool insertValues) //Atualiza os slots, checando se quer armazenar os valores pra ficarem salvos e/ou puxar os valores salvos dos itens que o player tem 
    {
        foreach(KeyValuePair<int, InventorySlot> slot in inventory) //Pega posição por posição do dicionario
        {
            slot.Value.UpdateSlot(insertValues); //Atualiza as posições
        }
    }

    public void UpdateHotbarSlots(bool insertValues) //Atualiza os slots, checando se quer armazenar os valores pra ficarem salvos e/ou puxar os valores salvos dos itens que o player tem 
    {
        foreach(var slot in hotbar) //Mesma coisa do de cima soq de um jeitinho diferente, pra ficar anotado
        {
            slot.Value.UpdateSlot(insertValues);
        }
        
        //UpdateHotbarPlayer(false);
    }
    public void UpdateHotbarPlayer() //Atualiza os slots, checando se quer armazenar os valores pra ficarem salvos e/ou puxar os valores salvos dos itens que o player tem 
    {
        for(int i =0; i <hotbarAmount; i ++)
        {
            hotbarPlayer[i].item = hotbar[i].item; //Copia os itens da hotbar do inventario pra hotbar do player
        }    
        foreach(var slot2 in hotbarPlayer)
        {
            slot2.Value.UpdateSlot(false);
        }

    }
    public void UpdateWeaponsSlots(bool insertValues) //Atualiza os slots, checando se quer armazenar os valores pra ficarem salvos e/ou puxar os valores salvos dos itens que o player tem 
    {
        foreach(var slot in weapons)
        {
            slot.Value.UpdateSlot(insertValues);
        }
    }

    public void OnDropDone(InventorySlot drop, int idSlot, SlotType stype) //troca os objetos movimos de lugar(Final do drag & drop)
    {
        slotDrop = drop;

        try
        {

        Item iDrag = slotDrag.item;
        Item iDrop = slotDrop.item;

        if(slotDrop.slotType != SlotType.WEAPON && iDrag != null && iDrag.canMove == true && iDrop.canMove == true) //Checa se algum item n pode mover ou se o item movido é nulo e se nenhum dos objetos é das parte das armas
        {
            slotDrag.item = iDrop;
            slotDrop.item = iDrag;
            UpdateInventorySlots(false);
            UpdateHotbarSlots(false);
            SelectItem(idSlot, stype);
        }
        }
        catch{}
        slotDrag = null;
        slotDrop = null;
    }

    public void ChangeDescPanel(Item item, int language) //Atualiza o nome e descrição do item no painel de desc
    {
        inventoryDesc.ChangeDesc(item.itemName[language], item.itemDesc[language], item.itemSprite);
    }

    public void DestroyItem(int idSlot, SlotType stype) //Destroi o item
    {
        if(stype == SlotType.HOTBAR) //se for da hotbar destroi o item da hotbar
        {
            hotbar[idSlot].item = itemEmpty;
            UpdateHotbarSlots(false);
        }
        if(stype == SlotType.INVENTORY) //se for do inventario destroi o item do inventario
        {
            inventory[idSlot].item = itemEmpty;
            UpdateInventorySlots(false);
        }
        ChangeDescPanel(itemEmpty, 0); //Reseta as infos do painel de descrição para nulas
    }

    public void SelectItem(int slotId, SlotType sType) //Marca o slot que o player clickou como selecionado  
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
                    hotbar[i].itemSelected = true;
                    hotbar[i].EnableButtons(); //Habilita os botões de acordo qual pode ser habilitado (itens que podem ou n ser consumidos e/ou destruidos)
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
        UpdateWeaponsSlots(false);
    }
}
