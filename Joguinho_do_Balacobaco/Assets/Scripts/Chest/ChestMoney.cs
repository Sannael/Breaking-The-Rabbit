using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ChestMoney", menuName = "Scriptable/ChestMoney")]
public class ChestMoney : ScriptableObject 
{
    [Tooltip("A quantia de dinheiro é colocada, respeitando o valor minimo e maximo da dungeon, como seu lugar no array -1; ex: dungeon 1 (floresta) drop minimo é 10, e o maximo 25, ent moneymin[0] = 10, moneymac[0] = 25")]
    public int[] minMoney;
    public int[] maxMoney;

    public int DropCoins(int dungeon)
    {
        int drop = Random.Range(minMoney[dungeon-1], maxMoney[dungeon-1] +1);
        return drop;
    }
}

