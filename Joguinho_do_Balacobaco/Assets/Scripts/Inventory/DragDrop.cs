using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    public Canvas canvas;
    public InventorySlot slot;
    public RectTransform _transform;
    public Image itemDrag;
    public bool isDrag;
    public int language;
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }
    void Update()
    {
        if(isDrag == false && itemDrag != null)
        {
            itemDrag.sprite = null;
            itemDrag.enabled = false;
            itemDrag = null;
            CoreInventory._instance.inventory.slotDrag = null;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        /*if(CoreInventory._instance.inventoryDescs.move == true)
        {
            CoreInventory._instance.inventory.slotDrag.item = CoreInventory._instance.inventoryDescs.item;
            return;
        }*/
        if(slot.item.isEmpty == true)
        {
            return;
        }
        if(slot.slotType == SlotType.WEAPON)
        {
            CoreInventory._instance.inventory.ChangeDescPanel(slot.item, language);

            int ids = this.gameObject.GetComponent<InventorySlot>().idSlot; //dando ruim
            SlotType sTypes = this.gameObject.GetComponent<InventorySlot>().slotType;
            CoreInventory._instance.inventory.SelectItem(ids, sTypes);
            CoreInventory._instance.inventoryDescs.DisableAllButtons();
            return;
        }
        itemDrag = CoreInventory._instance.inventory.itemDrag;
        itemDrag.sprite = slot.item.itemSprite;
        itemDrag.enabled = true;
        itemDrag.rectTransform.position = transform.position;
        itemDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
        isDrag = true;
        
        
        int id = this.gameObject.GetComponent<InventorySlot>().idSlot; //dando ruim
        SlotType sType = this.gameObject.GetComponent<InventorySlot>().slotType;
        CoreInventory._instance.inventory.SelectItem(id, sType);
    
        CoreInventory._instance.inventory.slotDrag = slot;

        CoreInventory._instance.inventory.ChangeDescPanel(slot.item, language);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(itemDrag != null)
        {
            itemDrag.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(itemDrag != null)
        {
            itemDrag.enabled = false;
            itemDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
            itemDrag = null;
        }
        isDrag = false;
    }
}
