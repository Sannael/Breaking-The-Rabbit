using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    public PolygonCollider2D[] polCollider; //1 ou mais colisores da arma
    public Animator meleeAnim; 
    public GameObject gunCase;
    void Start()
    {
        gunCase = gameObject.GetComponentInParent<MeleeController>().gunCase;
        meleeAnim.SetTrigger("Attack");
    }
    void Update()
    {  
    }

    public void DisableCollider(int collider) //Desativar o colisor
    {
        polCollider[collider].enabled = false;
    }
    public void EnableCollider(int collider) //Ativar o colisor
    {
        polCollider[collider].enabled = true;
    }
    public void EndOfAtk()
    {
        gunCase.SetActive(true);
        gameObject.SetActive(false);
    } 
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("EnemyAtk"))
        {
            Destroy(other.gameObject);
        }    
    }
}
