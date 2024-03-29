using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeItem : MonoBehaviour
{
    public Item item;
    public InputActionReference interaction;
    public GameObject uiTakeItem;
    public bool canTake = false;
    private bool seeUi = false;
    private bool take = false;
    private GameObject uiTake;
    void Update()
    {
        
        if(canTake == true && seeUi == false)
        {
            seeUi = true;
            uiTake = Instantiate(uiTakeItem, transform.position, Quaternion.identity); //Aparece a UI de troca de arma
            uiTake.transform.parent = transform;
        }
        if(canTake == false && seeUi == true)
        {
            seeUi = false;
            Destroy(uiTake);
        }

        if(interaction.action.IsPressed() && take == false && canTake == true)
        {
            take = true;
            canDestroy(CoreInventory._instance.inventory.GetItem(item, item.itemAmount, item.unique, item.pileable, item.weapon));
        }

        if(interaction.action.IsPressed() == false && take == true)
        {
            take = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && canTake == false)
        {
            canTake = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canTake = false;
            take = false;
        }
    }

    void canDestroy(bool destroy)
    {
        if(destroy == true)
        {
            Destroy(this.gameObject);
        }
        else
        {
            GameObject.Find("Player Hotbar Panel").GetComponent<Animator>().SetTrigger("Shake");
        }
    }
}
