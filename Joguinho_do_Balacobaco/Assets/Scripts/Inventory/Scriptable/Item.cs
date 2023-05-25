using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="item", menuName ="Scriptable/Item", order =1)]
public class Item : ScriptableObject
{
    public int itemId;
    public bool isEmpty;
    public string[] itemName;
    public Sprite itemSprite;
    public int itemAmount;
    public int itemStak;
    public int maxItens;
    public int maxStak;
    [Header("Drop in")]
    public bool chest;
    public bool shop;
    [Header("Interactive info")]
    public bool consumible; 
    public bool canMove; 
    public bool canDestroy;
    
    [Tooltip("Unique itens são itens unicos (Não podem ter mais de um no inventario);    Pileable itens são itens que podem estacar entre si (itens que voce pode 'juntar' varios no mesmo slot);   Unique pileable itens (Marcar as duas opções) são itens 'semi-unicos'(player pode ter mais de um, em slots diferentes; somente 1 por slot)  Stakable são itens que só podem ter um no slot, porém tem uma passiva que estaca de alguma maneira")]
    [Header("Item Type")]
    public bool unique; 
    public bool pileable;
    public bool stakable;

    [Header("Description")]
    [TextArea(10,7)]
    public string[] itemDesc;

}
