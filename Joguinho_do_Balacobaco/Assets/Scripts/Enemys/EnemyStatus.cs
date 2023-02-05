using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health;
    public bool canTakeDmg = true;
    public bool isAlive;
    public Animator slimeAnimator;


    void Start()
    {
        canTakeDmg = true;
        isAlive = true;
        slimeAnimator = this.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if(health <= 0)
        {
            slimeAnimator.SetTrigger("Death");
        }
        if(isAlive == false)
        {
            Destroy();
        }

        if(canTakeDmg == false)
        {
            canTakeDmg = true;
            //StartCoroutine(ResetTakeDmg()); //caso o inimigo tenha um tempo de invencibilidade pós ataque
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D (Collider2D other) 
    {
        if(other.gameObject.tag == "PlayerBullet" && canTakeDmg == true)
        {
            canTakeDmg = false;
            health = health - other.gameObject.GetComponent<DamageScript>().damage;
        }
    }

    public IEnumerator ResetTakeDmg()
    {
        yield return new WaitForSeconds(0.01f);
        canTakeDmg = true;
        
    } 
}
