using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    public Canvas canvas; //Canvas ;)
    public InventorySlot slot; //Slots do inventario ou hotbar
    public RectTransform _transform;
    public Image itemDrag; //imagem do item que estiver sendo movido
    public bool isDrag; //Ta movendo?
    public int language; //Checa linguagem, 0 = pt, 1 = eng e etc
    public bool canMove; //Checa se o item pode ser movido
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }
    void Update()
    {
        if(isDrag == false && itemDrag != null) //Caso nenhum item esteja sendo movido 
        {
            itemDrag.sprite = null; //desabilita a imagem
            itemDrag.enabled = false;
            itemDrag = null;
            CoreInventory._instance.inventory.slotDrag = null; //Deixa o slot de item sendo puxado pra nulo
        }
        canMove = this.GetComponent<InventorySlot>().canMove;
    }


    public void OnPointerDown(PointerEventData eventData) //Evento OnClick
    {
        if(slot.item.isEmpty == true) //Se Clickar em um slot vazio só ignora
        {
            return;
        }
        if(slot.slotType == SlotType.WEAPON) //Se o slot for de arma, puxa as informações pra jogar no painel de descs 
        {
            CoreInventory._instance.inventory.ChangeDescPanel(slot.item, language); //Muda as infos do painel de acordo com o item que foi clickado

            int ids = this.gameObject.GetComponent<InventorySlot>().idSlot; 
            SlotType sTypes = this.gameObject.GetComponent<InventorySlot>().slotType;
            CoreInventory._instance.inventory.SelectItem(ids, sTypes); //Marca o slot como selecionado
            CoreInventory._instance.inventoryDescs.DisableAllButtons(); //Desabilita as interações de botões
            return;
        }
        if(canMove == false) //Se o item não poder ser movido, só joga as informações dele no painel de desc
        {
            CoreInventory._instance.inventory.ChangeDescPanel(slot.item, language); //Muda as infos do painel de acordo com o item que foi clickado

            int ids = this.gameObject.GetComponent<InventorySlot>().idSlot; 
            SlotType sTypes = this.gameObject.GetComponent<InventorySlot>().slotType;
            CoreInventory._instance.inventory.SelectItem(ids, sTypes); //Marca o slot como selecionado
            return;
        }
                    //Caso n entrar em outra opção
        itemDrag = CoreInventory._instance.inventory.itemDrag; //Salva qual item esta sendo movido
        itemDrag.sprite = slot.item.itemSprite; //deixa a arte do itemque esta sendo movido igual a arte do item que foi clickado
        itemDrag.enabled = true; //Habilita que imagem do item apareça
        itemDrag.rectTransform.position = transform.position; //O item segue o mouse
        itemDrag.GetComponent<CanvasGroup>().blocksRaycasts = false; //Ignora o Raycast pra poder seguir o mouse sem delay
        isDrag = true;
        
                                    //Armazena algumas infos do item que ta sendo movido
        int id = this.gameObject.GetComponent<InventorySlot>().idSlot; 
        SlotType sType = this.gameObject.GetComponent<InventorySlot>().slotType;
        CoreInventory._instance.inventory.SelectItem(id, sType); //Marca o slot como selecionado
    
        CoreInventory._instance.inventory.slotDrag = slot; //Marca o slot que foi selecionado

        CoreInventory._instance.inventory.ChangeDescPanel(slot.item, language); //Muda as descrições do painel
        itemDrag.preserveAspect = true;
    }

    public void OnPointerUp(PointerEventData eventData) //Quando botao do mouse "Subir"
    {
        isDrag = false; //Nenhum item sendo movido
    }

    public void OnDrag(PointerEventData eventData) //Quando tiver arrastando o mouse pela tela
    {
        if(itemDrag != null)  //Se tiver item sendo movido
        {
            itemDrag.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //Segue o mouse sem dor de cabeça (Usa escala do monitor)
        }
        
    }

    public void OnEndDrag(PointerEventData eventData) //Qaundo parar de arrastar
    {
        if(itemDrag != null)  //Se tiver item sendo movido, basicamente reseta os valores
        {
            itemDrag.enabled = false;
            itemDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
            itemDrag = null;
        }
        isDrag = false;
    }
}
