using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pnlPause, pnlControler;
    
    void Start()
    {  
    }
    
    void Update()
    {
    }

    public void OpenPanelControls()
    {
        pnlControler.SetActive(true);
        try{pnlPause.SetActive(false);}catch{}
    }
    public void ClosePanelControls()
    {
        pnlControler.SetActive(false);
        try{ pnlPause.SetActive(true);}catch{}
    }
}
