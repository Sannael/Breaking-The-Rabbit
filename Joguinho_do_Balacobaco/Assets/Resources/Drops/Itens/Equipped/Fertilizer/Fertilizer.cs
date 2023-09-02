using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fertilizer", menuName = "ScriptableItem/Fertilizer")]
public class Fertilizer : Item
{
    [Header("Fertilizer")]
    public int shopDiscount;

    public override void Use()
    {
        base.Use();
        if(used == false)
        {
            if (shopDiscount > GameObject.Find("Player").GetComponent<PlayerScript>().shopDiscount)
            {
                GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("shopDiscount", shopDiscount, false);
            }
        }
    }

    public override void DisUseItem()
    {
        base.DisUseItem();
        GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("shopDiscount", 0, false);
    }
}
