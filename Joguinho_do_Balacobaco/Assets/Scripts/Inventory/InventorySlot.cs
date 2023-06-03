using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class InventorySlot : MonoBehaviour, IDropHandler
{//Todos os nomes de variaveis que tem aqui, ja expliquei em outro script (maioria delas no mesmo alias), nome são bem objetivos tbm, mucho texto explicar de novo
    public InventorySave inventorySave; 
    public SlotType slotType;
    public int idSlot;
    public Item item;
    public int itemId;
    public Image itemImage;
    public int itemAmount;
    public int itemStak;
    public bool unique, pileable, stakable;
    public string itemDesc;
    public TMP_Text itemCount, itemStaktxt;
    public bool itemSelected;
    public bool consumible, canMove, canDestroy;
   void Update()
    {
        if(slotType == SlotType.PLAYERHOTBAR)
        {
            GameObject playerHotbarPanel = GameObject.Find("Player Hotbar Panel");
            UpdateColor(playerHotbarPanel.GetComponentInParent<Image>().color);
        }
        else if(itemSelected == true)
        {
            UpdateColor(new Color32(130, 130, 130, 225));
        }
        else
        {
            UpdateColor(new Color32(255, 255, 255, 255));
        }
    }
    public void UpdateColor(Color32 color)
    {
        this.GetComponent<Image>().color = color;
    }
    public void CreateSlot(int id, Item i) //Cria o slot e adiciona o item a ele (inicialmente Empty)
    {
        idSlot = id;
        item = i;
    }
    public void UpdateSlot(bool insert) //Atualiza o slot, cehcando se inseri valor novo ou se puxa valores ja existentes sobre itens que o player tem
    {
        if(item.isEmpty == true) //Se for empty deixa todas as infos nulas
        {
            itemId = 0;
            itemImage.sprite = null;
            itemImage.enabled = false;
            itemCount.enabled = false;
            itemStak = 0;
            itemAmount = 0;
            unique = item.unique;
            pileable = item.pileable;
            stakable = item.stakable;
            consumible = item.consumible;
            canMove = item.canMove;
            canDestroy = item.canDestroy;
        }
        else //Se n for vazio, basicamente puxa todas as informações do item
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
            itemAmount = 1;
            if(unique == false) 
            {
                itemCount.enabled = true; //Habilita o texto que armazena a quantidade que o player tem desse item
                bool parse = int.TryParse(CoreInventory._instance.inventory.GetItemAmount(item), out itemAmount); //tenta converter pra int
                itemCount.text = itemAmount.ToString(); //e depois converte pra string; parece meio sem nexo, eu sei, mas se eu n fizer essa etapa assim, 
                //pode dar algum erro na horas de passar pra int, pra quando for salvar a quantidade que o player tem do item         
            }
            else
            {
                itemCount.text = "";
            }
            if(stakable == true)
            {
                itemStaktxt.enabled = true;
                itemStaktxt.text = itemStak.ToString();
            }
            if(itemStak == 0)
            {
                itemStaktxt.text = "";
            }
        }

        if(insert == true) //Armazena as informações
        {
            inventorySave.InsertItemInfos(idSlot, item.itemId, itemAmount, itemStak, item);
        }

    }
    public void ReUpdateSlot() //Aqui é basicamente a função de cima, só que uso sem inserir nem puxar infos salvas, e depois armazeno o item no dicionario
    { //Uso outra função pra basicamente n ter problema de bugar os itens no dicionario. Só uso quando puxo informações de qual item o player tem
        if(item.isEmpty == true)
        {
            return;
        }
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
        if(stakable == true)
        {
            itemStaktxt.enabled = true;
            itemStaktxt.text = itemStak.ToString();
        }
        if(itemStak == 0)
        {
            itemStaktxt.text = "";
        }
        
        CoreInventory._instance.inventory.ReGetItem(slotType, idSlot, item, itemAmount, unique, pileable, item.weapon);
        
    }
    public void ResetItensInfo() //Aqui puxa as informações dos itens que o player tinha (quase um Backup)
    {
        if(slotType != SlotType.PLAYERHOTBAR)
        {
            int[] values = inventorySave.GiveItemInfos(idSlot);
            itemAmount = values[0];
            itemStak = values[1];
            item = inventorySave.TakeValues(idSlot);
            ReUpdateSlot();
        }
    }
    public void EnableButtons() //Habilita os botões e passa algumas informações pro painel de descrições
    {
        CoreInventory._instance.inventoryDescs.EnableButtons(consumible, canDestroy);

        CoreInventory._instance.inventoryDescs.item = item;
        CoreInventory._instance.inventoryDescs.idSlot = idSlot;
        CoreInventory._instance.inventoryDescs.slotType = slotType;

    }
    public void OnDrop(PointerEventData eventData) //Faz parte do Drag & Drop, quando terminar de arrastar o item
    {
        
        CoreInventory._instance.inventory.OnDropDone(this, idSlot, slotType); //Puxa esse item e passa pro inventario, pra ver se vai rolar de trocar os itens de lugar 
    }
}
