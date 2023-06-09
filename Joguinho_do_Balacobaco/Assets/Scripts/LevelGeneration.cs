 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public bool maybeEnemy;
    public GameObject[] objects;
    public int enemyChance;
    void Start()
    {
        if(maybeEnemy == true)
        {
            float may = Random.Range(0, 100);
            if(may > enemyChance)
            {
                int rand = Random.Range(0, objects.Length);
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
