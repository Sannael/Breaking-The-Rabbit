using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PnlDestroy : MonoBehaviour
{
    public TextMeshProUGUI text; //Lugar onde aparece o texto de tem certeza
    public int lang; //Idioma
    [TextArea(10,7)]
    public string[] part1; //começo do texto
    public string itemname;
    private void OnEnable() 
    {
        text.text = part1[lang] + itemname.ToUpper() + " ?"; //O texto que aparece pro player
    }
}
