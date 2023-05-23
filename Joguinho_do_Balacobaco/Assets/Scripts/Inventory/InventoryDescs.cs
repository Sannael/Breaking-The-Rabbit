using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryDescs : MonoBehaviour
{
    public TMP_Text itemName, ItemDesc;
    public GameObject descPanel;
    public Image itemImage;
    [Header("Buttons")]
    public Button btnConsume;
    public Button btnDestroy;
    [Header("Selected item Informations")]
    public Item item;
    public int idSlot;
    public SlotType slotType;

    void Start()
    {
        DisableAllButtons();
    }
    void Update()
    {
    }
    public void ChangeDesc(string name, string desc, Sprite itemImg)
    {
        itemName.text = name;
        ItemDesc.text = desc;
        itemImage.sprite = itemImg;
        itemImage.enabled = true;
    }

    public void EnableButtons(bool consume, bool destroy)
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
    }

    public void DisableInfos()
    {
        ChangeDesc("","",null);
        itemImage.enabled = false;
        CoreInventory._instance.inventory.SelectItem(-1, SlotType.WEAPON);
        item = CoreInventory._instance.inventory.itemEmpty;
    }

    public void ClickDestroy()
    {
        CoreInventory._instance.inventory.DestroyItem(idSlot, slotType);
        DisableAllButtons();
    }
}
