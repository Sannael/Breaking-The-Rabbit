using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensChestDrop : MonoBehaviour
{
    public Animator chestAnim;
    public ChestItens chestItens;
    private bool candrop;
    private PlayerScript ps;
    private bool alreadyDrop; 
    public GameObject uiOpenChest;
    private bool uiChest;
    private GameObject ui;
    [Header("Sounds")]
    public AudioClip openSound;
    [Header("Chest Type")]
    [Tooltip("Tipo do baú é baseado, noq ele dropa: 0 = itens; 1 = armas de fogo; 2 = arma Melee 3 = tudo")]
    public int dropChestType;
    private void Start() 
    {
        alreadyDrop = false;
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    private void Update() 
    {
        if(ps.interaction.action.IsPressed() && candrop == true && alreadyDrop == false)
        {
            candrop = false;
            chestAnim.SetTrigger("Open"); 
            GameSounds.instance.CreateNewSound(openSound);
        }
        if(candrop == true && alreadyDrop == false && uiChest == false)
        {
            uiChest = true;
            ui = Instantiate(uiOpenChest);
            ui.transform.position = transform.position;
        }
        else if(candrop == false || alreadyDrop == true)
        {
            if(uiChest == true)
            {
                uiChest = false;
                Destroy(ui);
            }
        }
    }


    private void CanDrop()
    {
        Item drop = chestItens.ChestDrop(dropChestType);
        if(drop != null)
        {
            DropItem(drop);
        }
        else
        {
            CanDrop();
        }
    }
    private void DropItem(Item drop)
    {
        GameObject obj = Instantiate(drop.thisPrefabDrop);
        obj.transform.position = transform.position;
        StartCoroutine(ItemDropAnim(obj));
        //Vector3 v = transform.position;
        //v[0] += 3;
        //obj.transform.position = v;
        alreadyDrop = true;
    }

    private IEnumerator ItemDropAnim(GameObject itemdrop)
    {
        Vector2 pos = new Vector2(0,0);
        int rand = Random.Range(0, 10);
        if(rand< 5)
        {
            pos = new Vector2(5,2);
        }
        else
        {
            pos = new Vector2(-5,2);
        }
        if(itemdrop.GetComponent<Rigidbody2D>() == false)
        {
            itemdrop.AddComponent<Rigidbody2D>();
        }
        itemdrop.GetComponent<Rigidbody2D>().velocity = pos;
        yield return new WaitForSeconds(0.1f); 
        pos[1] = 0;
        itemdrop.GetComponent<Rigidbody2D>().velocity = pos;
        yield return new WaitForSeconds(0.2f); 
        itemdrop.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0); 
        itemdrop.GetComponent<Rigidbody2D>().gravityScale = 0; 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            candrop = true;
        }   
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            candrop = false;
        }
    }
}
