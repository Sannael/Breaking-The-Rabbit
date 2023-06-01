using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonoTextLang", menuName = "Scriptable/MonoTextLang")]
public class MonoTextLang : ScriptableObject
{
    [TextArea(10,7)]
    public string[] text;

    public string returnText(int language)
    {
        return text[language];
    }
}
