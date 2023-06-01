using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquippedItem", menuName = "Scriptable/Equipped")]
public class ItensEquipped : Item
{
    public enum equipType{magnet};
    public equipType equipmentType;

    /*public override void Use()
    {
        base.Use();
        switch (equipmentType)
        {
            case equipType.magnet:
            Magnet();
            break;
        }
    }
*/
    void Magnet()
    {
        Debug.Log("HIHI");
    }
}
