using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryDescs : MonoBehaviour
{
    public TMP_Text itemName, ItemDesc;
    public Image itemImage;
    [Header("Buttons")]
    public Button btnConsume;
    public Button btnMove;
    public Button btnDestroy;
    public Item item;
    public bool move;

    void Start()
    {
        DisableAllButtons();
    }
    void Update()
    {
        if(btnMove.interactable == true || item == null)
        {
            move = false;
        }
    }
    public void ChangeDesc(string name, string desc, Sprite itemImg)
    {
        itemName.text = name;
        ItemDesc.text = desc;
        itemImage.sprite = itemImg;
        itemImage.enabled = true;
    }

    public void EnableButtons(bool consume, bool move, bool destroy)
    {
        print(consume + " " + move + " " + destroy);
        btnConsume.interactable = consume;
        btnMove.interactable = move;
        btnDestroy.interactable = destroy;
    }
    public void MoveClick()
    {
        move = true;
    }
    
    public void DisableAllButtons()
    {
        btnConsume.interactable = false;
        btnMove.interactable = false;
        btnDestroy.interactable = false;
    }

    private void OnDisable() 
    {
        ChangeDesc("","",null);
        itemImage.enabled = false;
        CoreInventory._instance.inventory.SelectItem(-1, SlotType.WEAPON);
        DisableAllButtons();
    }
}
