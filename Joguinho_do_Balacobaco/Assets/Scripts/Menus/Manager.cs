using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private InputActionReference esc;
    public Texture2D[] cursorTexture; //Arte do cursor
    public Vector2[] cursorHotspot; //HotSpot X e Y do cursor
    public GameController gameControllerScript;
    
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Main Menu") 
        {
            Time.timeScale = 1f;
        }
         
        try //Testando try catch (Nunca aprendi isso direito) 
        {
            gameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        }
        catch
        {

        }
    }
    // Update is called once per frame
    void Update()
    {    
        if(esc.action.IsPressed()) //pressionando a tecla ESC
        {
            if(SceneManager.GetActiveScene().name == "Credits") //checa se ta na tela de credits,
            {
                LoadScene(0);
            }
        }

        if(SceneManager.GetActiveScene().name == "Credits" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            ChangeCursor(1);
        }
        if(SceneManager.GetActiveScene().name == "CenaBruno" ||SceneManager.GetActiveScene().name == "CenaGuima" || SceneManager.GetActiveScene().name == "CenaPaula")
        {
            if(gameControllerScript.isPaused == true)
            {
                ChangeCursor(1);
            }
            else
            {
                ChangeCursor(0);
            }
        }
    }
    public string ReturnActivedSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene); //metodo que troca de cena (sem transição)
        /*
        Scenes (Seguindo a sequencia que tem na Build Settings, parte de scenes in build)
        0 = Main Menu
        1 = Credits
        2 = JogoGameJam
        3 = CenaGuima
        4 = CenaBruno

        */
    }

    public void ChangeCursor(int cursorID)
    {
        Cursor.SetCursor(cursorTexture[cursorID], cursorHotspot[cursorID], CursorMode.Auto); //Muda a arte e seta o hotspot do cursor
    }
    public void ClickStart()
    {
        LoadScene(2); 
    } 
    
    public void GoToCredits()
    {
        LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
