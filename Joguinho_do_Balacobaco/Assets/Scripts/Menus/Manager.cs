using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //pressionando a tecla ESC
        {
            if(SceneManager.GetActiveScene().name == "Credits") //checa se ta na tela de credits,
            {
                LoadScene(0);
            }
        }

    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene); //metodo que troca de cena (sem transição)
        /*
        Scenes (Seguindo a sequencia que tem na Build Settings)
        0 = Main Menu
        1 = Credits
        2 = Stage1

        */
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
