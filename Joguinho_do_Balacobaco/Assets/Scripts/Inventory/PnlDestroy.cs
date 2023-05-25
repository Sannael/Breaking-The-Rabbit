using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PnlDestroy : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int lang;
    [TextArea(10,7)]
    public string[] part1;
    public string itemname;
    private void OnEnable() 
    {
        text.text =   part1[lang] + itemname + " ?"; 
    }
}
