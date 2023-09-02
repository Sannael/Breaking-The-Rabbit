using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


public class AmmoScript : MonoBehaviour
{
    public GunStatus gunStatus;
    public TMP_Text ammoGunTxt, playerAmmoTxt;
    public Image gunImage;

    void Start()
    {
        
    }

    void Update()
    {
        try
        {
            gunStatus = GameObject.Find("Player").GetComponentInChildren<GunStatus>();
            gunImage.sprite = gunStatus.item.itemSprite;
            ammoGunTxt.text = gunStatus.ammo.ToString() + " / " + gunStatus.totalAmmo.ToString();
            playerAmmoTxt.text = gunStatus.playerAmmo.ToString();
        }
        catch{}
        
    }
}
