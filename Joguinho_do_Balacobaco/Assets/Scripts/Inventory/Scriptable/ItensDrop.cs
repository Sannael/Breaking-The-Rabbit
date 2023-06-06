using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItensDrop", menuName = "Scriptable/ItensDrop")]
public class ItensDrop : ScriptableObject
{
    public Dictionary<int, Item> itensDrop = new Dictionary<int, Item>();
}
