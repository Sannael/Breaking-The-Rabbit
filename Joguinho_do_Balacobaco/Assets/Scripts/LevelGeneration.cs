 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject[] objects;
    [Header("Enemys")]
    public bool isEnemy;
    public int[] enemysTypes;
    public bool maybeEnemy;
    public int enemyChance;
    public int dungeon;
    void Start()
    {
        if(isEnemy == true)
        {
            if(maybeEnemy == true)
            {
                float may = Random.Range(0, 100);
                if(may > enemyChance)
                {
                    int rand = Random.Range(0, enemysTypes[dungeon-1]);
                    Instantiate(objects[rand], transform.position, Quaternion.identity);
                }
            }
            else
            {
                int rand = Random.Range(0, enemysTypes[dungeon-1]);
                Instantiate(objects[rand], transform.position, Quaternion.identity);
            }
        }
        else
        {
            int rand = Random.Range(0, objects.Length);
            Instantiate(objects[rand], transform.position, Quaternion.identity);
        }
    }
}
