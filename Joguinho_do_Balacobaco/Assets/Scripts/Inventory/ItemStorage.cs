using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemStorage : MonoBehaviour
{
    public string[] itemN, itemD;
    public Sprite sprite;
    public Item itemz, item;

    public Dictionary<int, Item> dictionaryItens = new Dictionary<int, Item>();

    void Start()
    {
        dictionaryItens.Add(item.itemId, item);
        foreach(KeyValuePair<int, Item> item in dictionaryItens)
        {
            itemz = item.Value;
        }
    }
}
