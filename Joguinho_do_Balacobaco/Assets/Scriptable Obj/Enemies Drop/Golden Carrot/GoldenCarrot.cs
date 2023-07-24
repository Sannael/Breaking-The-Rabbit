using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldenCarrot", menuName = "ScriptableDrop/GoldenCarrot")]
public class GoldenCarrot : Item
{
    public int health; 

    public override bool Consume()
    {
        base.Consume();
        HealHearth();
        return consumed;
    }
    public void HealHearth()
    {
        consumed = false;
        PlayerScript ps;
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        if(ps.health < ps.maxHealth)
        {
            ps.health += health;
            consumed = true;
        }
    }
}
