using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinUI : MonoBehaviour
{
    public TMP_Text coinTxt; //txt da quantia de gold
    private PlayerScript ps;
    void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        coinTxt.text = ps.coinCount.ToString();
    }
}
