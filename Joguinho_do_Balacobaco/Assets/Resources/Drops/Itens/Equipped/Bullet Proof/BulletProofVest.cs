using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Proof Vest", menuName = "ScriptableItem/Bullet Proof Vest")]
public class BulletProofVest : Item
{
    [Header("Bullet Proof Vest")]
    public int armor;
    
    public override void Use()
    {
        base.Use();
        if(used == false)
        {
            used = true;
            GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("armor", armor, false);
        }
    }

    public override void DisUseItem()
    {
        base.DisUseItem();
        used = false;
        GameObject.Find("Player").GetComponent<PlayerScript>().ChangeVarValues("armor", (-armor), false);
    }
}
