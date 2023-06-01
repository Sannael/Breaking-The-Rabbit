using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemStorage : MonoBehaviour
{
    //Isso aqui, ainda to vendo oq eu faço
    //Mas basicamente vai ser como um Armazem propriamente dito, um armazem que armazena todos  os itens do jogo
    //Acho que transformo isso em um scriptable, depois só crio um "Extensor" pra poder organizar ele e bla bla bla
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
