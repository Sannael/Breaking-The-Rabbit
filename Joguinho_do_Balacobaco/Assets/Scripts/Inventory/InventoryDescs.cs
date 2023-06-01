using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryDescs : MonoBehaviour
{
    public TMP_Text itemName, ItemDesc; //Nome e descrição do item
    public Image itemImage; //Imagem do item
    [Header("Buttons")]
    public Button btnConsume; //Botão de consumir
    public Button btnDestroy; //Botão de destruir
    [Header("Selected item Informations")]
    public Item item; //Item selecionado
    public int idSlot; //id do slot do item selecionado
    public SlotType slotType; //Tipo de slot do item selecionado
    public GameObject destroyPnl; //Painel de tem certeza que deseja destruir
    
    void Start()
    {
        DisableAllButtons(); //Desabilita a innteração com os botões
    }
    public void ChangeDesc(string name, string desc, Sprite itemImg) //Muda tudo da desc do item
    {
        itemName.text = name;
        ItemDesc.text = desc;
        itemImage.sprite = itemImg;
        itemImage.enabled = true;
        destroyPnl.GetComponent<PnlDestroy>().itemname = name;
    }

    public void EnableButtons(bool consume, bool destroy) //Habilita os botões permitidos
    {
        btnConsume.interactable = consume;
        btnDestroy.interactable = destroy;
    }
    
    public void DisableAllButtons()
    {
        btnConsume.interactable = false;
        btnDestroy.interactable = false;
    }
    private void OnEnable() 
    {
        DisableInfos();
        DisableAllButtons();
    }

    private void OnDisable() 
    {
        DisableInfos();
        DisableAllButtons();
        EnableDisableDestPnl(false);
    }

    public void DisableInfos() //Tira a info do painel, qnd fechar o painel
    {
        ChangeDesc("","",null);
        itemImage.enabled = false;
        CoreInventory._instance.inventory.SelectItem(-1, SlotType.WEAPON);
        item = CoreInventory._instance.inventory.itemEmpty;
    }

    public void EnableDisableDestPnl(bool enable)
    {
        destroyPnl.SetActive(enable); //Ativa o painel de tem certeza que deseja deletar
    }

    public void ClickDestroy()
    {
        CoreInventory._instance.inventory.DestroyItem(idSlot, slotType); //Deleta o item
        DisableAllButtons();
    }
}
