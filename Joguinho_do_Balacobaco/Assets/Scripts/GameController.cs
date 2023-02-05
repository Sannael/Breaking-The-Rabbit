using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool roomState;
    public GameObject roots;
    public int numberOfEnemies;
    // Start is called before the first frame update
    private void Awake()
    {
        numberOfEnemies = 0;
        roomState = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        

        if (numberOfEnemies <= 0)
        {
            try
            {
                Destroy(GameObject.FindGameObjectWithTag("Roots"));
            }

            catch { Debug.Log("cath"); }

        }

    }

    public void RoomChange()
    {
        Debug.Log(numberOfEnemies);
        if (numberOfEnemies >= 1)
        {
            roomState = true;
        }
        else
        {
            roomState = false;
        }
        if (roomState)
        {
            Vector3 SpawnPoint = Camera.main.GetComponent<CameraController>().newCameraPos + new Vector3(0f, 0f, 2f);
            Instantiate(roots, SpawnPoint, Quaternion.identity);
        }
    }


}
