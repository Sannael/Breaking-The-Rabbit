using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingCrown : MonoBehaviour
{   
    private bool canTake;
    public GameObject uiTake;
    private GameObject ui;
    private bool seeUI;
    private bool taking = false;
    private PlayerScript ps;
    public Animator anim;
    public GameObject kingSlime;
    private GameObject player;
    private void Start() 
    {
        player = GameObject.Find("Player"); 
        ps = player.GetComponent<PlayerScript>();
    }
    private void Update() 
    {
        if(taking == true)
        {
            try{Destroy(ui);}
            catch{}
        }
        else
        {
            if(canTake == true && seeUI == false)
            {
                seeUI = true;
                ui = Instantiate(uiTake); 
                Vector3 pos = this.transform.position;
                pos[1] += 0.1f;
                ui.transform.position = pos;
            }    
            if(canTake == false && seeUI == true)
            {
                seeUI = false;
                Destroy(ui);
            } 
            if(ps.interaction.action.IsPressed() && canTake == true && ps.isRolling == false)
            {
                anim.SetTrigger("Destroy");
                taking = true;
                SpawnKingSlime();
            }
        } 
    }

    private void SpawnKingSlime()
    {
        GameObject.Find("GameController").SetActive(false);
        ps.enabled = false;

        GameObject gun = player.GetComponentInChildren<BarrelGunScript>().gun;
        GameObject barrelGun = player.GetComponentInChildren<BarrelGunScript>().barrel;
        bool direita = player.GetComponentInChildren<BarrelGunScript>().direita;

        GameObject.Find("Music").SetActive(false);
        player.GetComponent<Animator>().Rebind();
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        player.GetComponentInChildren<GunStatus>().enabled = false;
        player.GetComponentInChildren<BarrelGunScript>().enabled = false;
        player.GetComponentInChildren<MeleeController>().enabled = false;
        gun.GetComponent<Animator>().Rebind();

        if(direita == true)
        {
            player.transform.Rotate(0f, 180f, 0f);
            gun.transform.Rotate(180f, 0f, 0f);
        }
        Vector3 direction = new Vector3(530, 489, 0);
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(direction) - barrelGun.transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * (Mathf.Rad2Deg);
        barrelGun.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
        kingSlime.GetComponent<KingSlime>().anim.SetTrigger("Spawn");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canTake = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canTake = false;
        }    
    }
}
