using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private bool roomState;
    [SerializeField]
    private InputActionReference pause; //Puxa o botão de pausar levando em conta o action q for 
    public GameObject roots;
    public int numberOfEnemies;
    public  bool isPaused; //Pause do game
    public GameObject pnlPause; //Painel de pause
    public Manager sceneManager;
    private bool canScape;
    private void Awake()
    {
        numberOfEnemies = 0;
        roomState = true;
    }

    void Start()
    {
        canScape = true;
        sceneManager = GameObject.Find("SceneManager").GetComponent<Manager>();
        isPaused = false;
    }

    void Update()
    {
        if(pause.action.IsPressed() && canScape == true)
        {
            canScape = false;
            if(isPaused == true)
            {
                if(pnlPause.activeInHierarchy)
                {
                    Resume();
                }
                
            }
            else
            {
                Pause();
            }
        }

        if(pause.action.IsPressed() == false) //Precisa soltar o esc pra poder pausar/despausar denovo (depois de ter feito uma vez)
        { //Sem isso caso o imbecil segurar o esc a tela tem um ataque epilético
            canScape = true;
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
