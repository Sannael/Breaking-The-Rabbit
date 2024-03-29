using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private bool roomState;
    public int dungeon;
    [SerializeField]
    private InputActionReference pause, openInventory; //Puxa o botão de pausar levando em conta o action q for
    public string cantos;
    public GameObject[] roots;
    public int numberOfEnemies;
    public  bool isPaused; //Pause do game
    public GameObject pnlPause; //Painel de pause
    public GameObject inventoryWindow;
    public Manager sceneManager;
    private bool canPause, canInventory, inventory;
    public DungeonsInfo dungeonsInfo;
    public GameObject moneyChest;
    private bool inCombat;
    public GameObject[] r;
    [Header("Sounds")]
    public AudioClip openInventorySound;
    public AudioClip closeInventorySound;
    private void Awake()
    {
        dungeon = dungeonsInfo.dungeon;
        numberOfEnemies = 0;
        roomState = true;
        OpenCloseInventory(true);
        OpenCloseInventory(false);
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

        if(openInventory.action.IsPressed() && inventory == true && canInventory == true && inCombat == false)
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
                r = GameObject.FindGameObjectsWithTag("Roots");
                foreach(var i in r)
                {
                    if(i.GetComponent<Animator>())
                    {
                        i.GetComponent<Animator>().SetTrigger("Death");
                    }
                }
            }
            catch {}
            if(inCombat == true)
            {
                inCombat = false;
                DropCoinChest();
            }
        }
    }
    public void ButtonCloseInventory()
    {
        if(isPaused == true)
        {
            OpenCloseInventory(false);
        }
    }
    public void DropCoinChest()
    {
        GameObject chest = Instantiate(moneyChest);
        chest.GetComponent<MoneyChestFall>().dungeon = dungeon;
    }
    public void RoomChange()
    {
        if (numberOfEnemies >= 1)
        {
            roomState = true;
            if(inCombat == false)
            {
                inCombat = true;
            }
        }
        else
        {
            roomState = false;
        }
        if (roomState)
        {
            Vector3 SpawnPoint = Camera.main.GetComponent<CameraController>().newCameraPos + new Vector3(0f, 0f, 2f);
            switch (cantos)
            {
                case "cantos_1.0 (UnityEngine.Sprite)":
                    Instantiate(roots[0], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.1 (UnityEngine.Sprite)":
                    Instantiate(roots[1], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.2 (UnityEngine.Sprite)":
                    Instantiate(roots[2], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.3 (UnityEngine.Sprite)":
                    Instantiate(roots[3], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.4 (UnityEngine.Sprite)":
                    Instantiate(roots[4], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.5 (UnityEngine.Sprite)":
                    Instantiate(roots[5], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.6 (UnityEngine.Sprite)":
                    Instantiate(roots[6], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.7 (UnityEngine.Sprite)":
                    Instantiate(roots[7], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.8 (UnityEngine.Sprite)":
                    Instantiate(roots[8], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.9 (UnityEngine.Sprite)":
                    Instantiate(roots[9], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.10 (UnityEngine.Sprite)":
                    Instantiate(roots[10], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.11 (UnityEngine.Sprite)":
                    Instantiate(roots[11], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.12 (UnityEngine.Sprite)":
                    Instantiate(roots[12], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.13 (UnityEngine.Sprite)":
                    Instantiate(roots[13], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.14 (UnityEngine.Sprite)":
                    Instantiate(roots[14], SpawnPoint, Quaternion.identity);
                    break;
                case "cantos_1.15 (UnityEngine.Sprite)":
                    Instantiate(roots[15], SpawnPoint, Quaternion.identity);
                    break;
            }
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
            try{GameSounds.instance.CreateNewSoundNoScale(openInventorySound);}catch{}
        }
        else
        {
            Time.timeScale = 1f;
            inventoryWindow.SetActive(false);
            CoreInventory._instance.inventory.itemDrag.enabled = false;
            try{GameSounds.instance.CreateNewSoundNoScale(closeInventorySound);}catch{}
        }
        
    }
    
    public void BackToMainMenu()
    {
        sceneManager.LoadScene(0);
    }

}
