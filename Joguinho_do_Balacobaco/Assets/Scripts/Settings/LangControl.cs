using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LangControl : MonoBehaviour
{
    public string lang;
    public TMP_Text[] texts;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lang == "PTBR")
        {
            PTBRTexts();
        }

        if(lang == "EN")
        {
            ENTexts();
        }
    }

    public void PTBRTexts()
    {
        texts[0].text = "Andar Para Cima";
        texts[1].text = "Andar Para Baixo";
        texts[2].text = "Andar Para a Esquerda";
        texts[3].text = "Andar Para a Direita";
    }

    public void ENTexts()
    {
        texts[0].text = "Move Up";
        texts[1].text = "Move Down";
        texts[2].text = "Move To Left";
        texts[3].text = "Move To Right";
    }
}
