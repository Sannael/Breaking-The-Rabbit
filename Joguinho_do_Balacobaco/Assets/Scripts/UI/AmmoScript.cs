using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class AmmoScript : MonoBehaviour
{
    public GunStatus gunStatus;
    public TMP_Text ammoGunTxt, playerAmmoTxt;

    void Start()
    {
        
    }

    void Update()
    {
        try
        {
            gunStatus = GameObject.Find("Player").GetComponentInChildren<GunStatus>();
            ammoGunTxt.text = gunStatus.ammo.ToString() + " / " + gunStatus.totalAmmo.ToString();
            playerAmmoTxt.text = gunStatus.playerAmmo.ToString();
        }
        catch{}
        
    }
}
