using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemEnemyDrop", menuName = "ScriptableDrop/ItemEnemyDrop")]
public class ItemEnemyDrop : ScriptableObject 
{
    public Item goldenCarrotBitten;
    public Item goldenCarrot;

    public GameObject DropGoldenCarrot(Transform enemyDrop)
    {
        GameObject drop;
        float rand = Random.Range(0, 100);
        if(rand <=70)
        {
            drop = Instantiate(goldenCarrotBitten.thisPrefabDrop, enemyDrop.position, Quaternion.identity);
        }
        else
        {
            drop = Instantiate(goldenCarrot.thisPrefabDrop, enemyDrop.position, Quaternion.identity);
        }
        return drop;
    }
    
}

