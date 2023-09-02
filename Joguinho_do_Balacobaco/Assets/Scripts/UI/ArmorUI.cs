using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmorUI : MonoBehaviour
{
    public TextMeshProUGUI armorTxt;

    private void Update() 
    {
        armorTxt.text = GameObject.Find("Player").GetComponent<PlayerScript>().armor.ToString();    
    }
}
