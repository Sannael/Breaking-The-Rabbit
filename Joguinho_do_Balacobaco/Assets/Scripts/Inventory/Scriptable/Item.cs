using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="item", menuName ="Scriptable/Item", order =1)]
public class Item : ScriptableObject
{
    public bool isEmpty;
    public string[] itemName;
    public Sprite itemSprite;
    public int itemAmount;
    public int maxItens;
    [Header("Drop in")]
    public bool chest;
    public bool shop;
    [Header("Interactive info")]
    public bool consumible; 
    public bool canMove; 
    public bool canDestroy;
    
    [Tooltip("Unique itens são itens unicos (Não podem ter mais de um no inventario);    Stakable itens são itens que podem estacar entre si (itens que voce pode 'juntar' varios no mesmo slot);   por fim Unique Stakables itens (Marcar as duas opções) são itens 'semi-unicos'(player pode ter mais de um, em slots diferentes; somente 1 por slot)")]
    [Header("Item Type")]
    public bool unique; 
    public bool stakeable;

    [Header("Description")]
    [TextArea(10,7)]
    public string[] itemDesc;

}
