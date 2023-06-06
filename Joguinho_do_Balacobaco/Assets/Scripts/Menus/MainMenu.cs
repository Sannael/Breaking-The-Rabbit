using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator sureAnim; //animator do prompt "Are You Sure?"
    public GameObject settings; //Painel de config
    
    void Start()
    {   
    }

    void Update()
    {
    }

    public void ClickExit()
    {
        if(sureAnim.GetBool("Visible") == false) //checa se o texto ta visivel, pra n repetir a animação
        {
            sureAnim.ResetTrigger("Down");
            sureAnim.SetTrigger("Up"); //gatilho para inicio de uma animação (Up do Sure) 
            sureAnim.SetBool("Visible", true); //define como visivel
        }
        
    }

    public void No()
    {
        sureAnim.ResetTrigger("Up");
        sureAnim.SetTrigger("Down"); //gatilho para inicio de uma animação (Down do Sure)
        sureAnim.SetBool("Visible", false); //define como invisivel
    }

    public void BackToMenu()
    {
        settings.SetActive(false); //desativa o painel de settings
    }
    public void OpenSettings()
    {
        settings.SetActive(true); //ativa o painel de settings
    }
}
