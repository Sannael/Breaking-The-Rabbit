using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private bool roomState;
    [SerializeField]
    private InputActionReference pause, openInventory; //Puxa o botão de pausar levando em conta o action q for 
    public GameObject roots;
    public int numberOfEnemies;
    public  bool isPaused; //Pause do game
    public GameObject pnlPause; //Painel de pause
    public GameObject inventoryWindow;
    public Manager sceneManager;
    private bool canPause, canInventory, inventory;
    private void Awake()
    {
        numberOfEnemies = 0;
        roomState = true;
    }

    void Start()
    {
        canPause = true;
        inventory = true;
        canInventory = true;
        sceneManager = GameObject.Find("SceneManager").GetComponent<Manager>();
        isPaused = false;
    }

    void Update()
    {
        if(pause.action.IsPressed() && canPause == true)
        {
            canPause = false;
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
            canPause = true;
        }

        if(openInventory.action.IsPressed() && inventory == true && canInventory == true)
        {
            inventory = false;
            if(isPaused == true)
            {
                OpenCloseInventory(false);
            }
            else
            {
                OpenCloseInventory(true);
            }

        }
        if(openInventory.action.IsPressed() == false)
        {
            inventory = true;
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
        canInventory = false;
        pnlPause.SetActive(true);
    }
    public void Resume()
    {
        isPaused = !isPaused;
        Time.timeScale = 1f;
        canInventory = true;
        pnlPause.SetActive(false);
    }

    public void OpenCloseInventory(bool open)
    {
        isPaused = !isPaused;
        if(open == true)
        {
            Time.timeScale = 0f;
            inventoryWindow.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            inventoryWindow.SetActive(false);
        }
        
    }
    
    public void BackToMainMenu()
    {
        sceneManager.LoadScene(0);
    }

}
