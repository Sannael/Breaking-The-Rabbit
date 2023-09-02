using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WholeStarFruit", menuName = "ScriptableItem/WholeStarFruit")]
public class WholeStarFruit : Item
{
    [Header("Star Fruit")]
    public int starFruitMaxAdd;
    public int starFruitCountAdd;

    public override void Use()
    {
        if(used == false)
        {
            used = true;
            base.Use();
            GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("starFruitMax", starFruitMaxAdd, false);
            GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("starFruitCount", starFruitCountAdd, false);
        }
    }

    public override void DisUseItem()
    {
        if(used == true)
        {
            used = false;
            base.DisUseItem();
            GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("starFruitMax", - starFruitMaxAdd, false);
            GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("starFruitCount", - starFruitCountAdd, false);
        } 
    }
}
