using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool canExit;
    public GameObject uiPortal;
    private GameObject ui;
    private bool seeUI;
    private PlayerScript ps;
    private int scene;
    private void Start() 
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();  
        scene = SceneManager.GetActiveScene().buildIndex;  
    }
    private void Update() 
    {
        if(ps.interaction.action.IsPressed() && canExit == true)
        {   
            SceneManager.LoadScene(scene +1);
        }
        if(canExit == true && seeUI == false)
        {
            seeUI = true;
            ui = Instantiate(uiPortal);
            ui.transform.position = transform.position;
        }
        if(canExit == false && seeUI == true)
        {
            seeUI = false;
            Destroy(ui);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            canExit = true;
        }    
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            canExit = false;
        }    
    }
}
