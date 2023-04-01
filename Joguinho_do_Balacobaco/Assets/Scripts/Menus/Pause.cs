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
        pnlPause.SetActive(false);
    }
    public void ClosePanelControls()
    {
        pnlControler.SetActive(false);
        pnlPause.SetActive(true);
    }
}
