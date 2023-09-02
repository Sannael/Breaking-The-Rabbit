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
    public bool goToCredits;
    public bool exitFloor;
    private bool createAnim;
    private void Start() 
    {
        exitFloor = false;
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();  
        scene = SceneManager.GetActiveScene().buildIndex;  
    }

    
    private void Update() 
    {
        if(exitFloor == true)
        {
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().exitTriggers >= 5)
            {
                SceneManager.LoadScene(scene +1);
            }
        }
        if(ps.interaction.action.IsPressed() && canExit == true)
        {  
            if(goToCredits == false)
            {
                ps.ChangeDungeonFloor();
                exitFloor = true;
                //ChangeScene();
                //SceneManager.LoadScene(scene +1);
            } 
            else
            {  
                SceneManager.LoadScene(1); 
            }
        }
        if(canExit == true && seeUI == false)
        {
            seeUI = true;
            ui = Instantiate(uiPortal);
            Vector3 pos = transform.position;
            pos[1] += 0.2f;
            ui.transform.position = pos;
        }
        if(canExit == false && seeUI == true)
        {
            seeUI = false;
            Destroy(ui);
        }
    }

    private void ChangeScene()
    {
        float wait = FadeInOut.instance.FadeIn();
        StartCoroutine(FadeTime(wait));
    }
    private IEnumerator FadeTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene +1);
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

    private void OnBecameVisible() 
    {
        if(createAnim == false)
        {
            createAnim = true;
            this.GetComponent<Animator>().SetTrigger("Create");
        }   
    }
}
