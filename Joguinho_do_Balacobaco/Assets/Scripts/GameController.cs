using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool roomState;
    public GameObject roots;
    public int numberOfEnemies;
    public  bool isPaused; //Pause do game
    public GameObject pnlPause; //Painel de pause
    public Manager sceneManager;
    private void Awake()
    {
        numberOfEnemies = 0;
        roomState = true;
    }

    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<Manager>();
        isPaused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
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

    public void Pause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0f;
        pnlPause.SetActive(true);
    }
    public void Resume()
    {
        isPaused = !isPaused;
        Time.timeScale = 1f;
        pnlPause.SetActive(false);
    }
    
    public void BackToMainMenu()
    {
        sceneManager.LoadScene(0);
    }

}
