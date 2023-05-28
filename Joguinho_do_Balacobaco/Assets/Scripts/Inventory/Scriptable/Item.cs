using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="item", menuName ="Scriptable/Item", order =1)]
public class Item : ScriptableObject
{
    public int itemId; //Id do item (não pode se repetir)
    public bool isEmpty; //Checa se o item é vazio (basicamente true só pro empty)
    public string[] itemName; //Nome do item, em array pra qnd ocorrer mudanças de idioma...[0] = pt, [1] = eng e etc
    public Sprite itemSprite; //Idle do item
    public int itemAmount; //Quantia inicial do item
    public int itemStak; //Stak inicial do item
    public int maxItens; //Quantia maxima que o player pode ter do item
    public int maxStak; //Quantia maxima que o player pode stakar do item 
    [Header("Drop in")]
    public bool chest; //Checa se é dropado em baús
    public bool shop; //Checa se é um item compravel
    public int value; //Preço do item na loja
    [Header("Interactive info")]
    public bool consumible; //Checa se o item é consumivel
    public bool canMove; //Checa se pode trocar o item de lugar
    public bool canDestroy; //Checa se o item pode ser destruido
    
    [Tooltip("Unique itens são itens unicos (Não podem ter mais de um no inventario);    Pileable itens são itens que podem estacar entre si (itens que voce pode 'juntar' varios no mesmo slot);   Unique pileable itens (Marcar as duas opções) são itens 'semi-unicos'(player pode ter mais de um, em slots diferentes; somente 1 por slot)  Stakable são itens que só podem ter um no slot, porém tem uma passiva que estaca de alguma maneira")]
    [Header("Item Type")]
    public bool unique; //Item é unico? (Somente um por inventario)
    public bool pileable; //Item é pilhavel? (Varios no mesmo slot)
    public bool stakable; ////Item é Stakavel? (tem uma passiva que é buffada dps de algum feito/tempo?)

    [Header("Description")]
    [TextArea(10,7)]
    public string[] itemDesc; //Descição do item, em array pra qnd ocorrer mudanças de idioma...[0] = pt, [1] = eng e etc
    [Header("Weapons")]
    public int weapon; //Se for uma arma ou carambola (arma de fogo = 1, corpo a corpo = 2, carambola = 3)
    public GameObject thisPrefabDrop; //Drop dp item, caso seja dropado pelo bau, e ou comprado na loja
}
