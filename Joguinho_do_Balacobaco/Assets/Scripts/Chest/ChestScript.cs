using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Linq;

public class ChestScript : MonoBehaviour
{
    public bool take = false;
    public bool candrop;
    public Item[] bau;
    private Item[] playerItens = new Item[6];
    public int price;
    private PlayerScript ps;
    private bool itensChecked = false;
    private void Start() 
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }
    void Update()
    {
        if(ps.interaction.action.IsPressed() && candrop == true && take == false)
        {
            take = true;
            ps = GameObject.Find("Player").GetComponent<PlayerScript>();
            RandomizeDrop();
        }
    }

    private void TryBuyChest()
    {
        float finalPrice = price;
        if(ps.shopDiscount > 0)
        {
            float playerDiscountPercent = (float)ps.shopDiscount / 100f;
            float playerDiscount = price * playerDiscountPercent;
            finalPrice = price - playerDiscount;
        }
        if(ps.coinCount >= finalPrice)
        {
            ps.coinCount -= (int)finalPrice;
            bau = Resources.LoadAll("Drops", typeof(Item)).Cast<Item>().ToArray();
            RandomizeDrop();
        }
        else
        {
        }
    }
    private void RandomizeDrop()
    {
        bool equals = false;
        int id = Random.Range(0, bau.Length);
        if(bau[id].weapon == 0)
        {Debug.Log(bau[id]);
            foreach(var i in CoreInventory._instance.inventory.hotbar)
            {
                playerItens[i.Key] = i.Value.item;
            }
            for(int i =0; i < playerItens.Length; i ++)
            {
                if(bau[id] == playerItens[i])
                {
                    equals = true;
                }
            }
        }
        else
        {
            equals = true;
        }

        if(equals == false)
        {
            Bau(id);   
        }
        else
        {
            RandomizeDrop();
        }
        
    }
    private void Bau(int drop)
    {
        candrop = false;
        GameObject obj = Instantiate(bau[drop].thisPrefabDrop); 
        Vector3 v = this.transform.position;
        v[0] += 3;
        obj.transform.position = v;
    }
}

