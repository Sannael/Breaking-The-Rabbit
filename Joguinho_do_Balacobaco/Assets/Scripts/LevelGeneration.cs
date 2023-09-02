 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
    private int dungeonMap; 
    public GameObject[] objects;
    public bool isMap;
    public int mapAmount;
    [Header("Enemys")]
    public bool isEnemy;
    public int[] enemysTypes;
    public bool maybeEnemy;
    public int enemyChance;
    public int dungeonFloor;
    private void Awake() 
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string[] scene = sceneName.Split(" ");  
        dungeonFloor = int.Parse(scene[1]); 
        
        dungeonMap = dungeonFloor -1;
    }
    void Start()
    {
        if(isEnemy == true)
        {
            if(maybeEnemy == true)
            {
                float may = Random.Range(0, 100);
                if(may > enemyChance)
                {
                    int rand = Random.Range(0, enemysTypes[dungeonMap]);
                    Instantiate(objects[rand], transform.position, Quaternion.identity);
                }
            } 
            else
            {
                int rand = Random.Range(0, enemysTypes[dungeonMap]);
                Instantiate(objects[rand], transform.position, Quaternion.identity);
            }
        }
        else if(isMap == true)
        {
            int rand= Random.Range(0, mapAmount);
            rand += (mapAmount * dungeonMap);
            Debug.Log(objects[rand]);
            Instantiate(objects[rand], transform.position, Quaternion.identity);
        }
        else
        {
            int rand = Random.Range(0, objects.Length);
            Instantiate(objects[rand], transform.position, Quaternion.identity);
        }
    }
}
