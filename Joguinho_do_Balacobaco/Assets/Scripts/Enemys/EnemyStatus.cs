using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health;
    public bool canTakeDmg = true;


    void Start()
    {
        canTakeDmg = true;
    }
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        if(canTakeDmg == false)
        {
            canTakeDmg = true;
            //StartCoroutine(ResetTakeDmg()); //caso o inimigo tenha um tempo de invencibilidade pós ataque
        }
    }

    public void OnTriggerEnter2D (Collider2D other) 
    {
        if(other.gameObject.tag == "PlayerBullet" && canTakeDmg == true)
        {
            canTakeDmg = false;
            health = health - other.gameObject.GetComponent<DamageScript>().damage;
            //health -= 1; 
        }
    }

    public IEnumerator ResetTakeDmg()
    {
        yield return new WaitForSeconds(0.01f);
        canTakeDmg = true;
        
    }

    private void OnBecameVisible() //Quando o objeto se tornar visivel em qualquer camera do jogo (A do scene conta)
    {
        GameObject.Find("GameController").GetComponent<GameController>().numberOfEnemies += 1;
    }

    private void OnDestroy()
    {
        GameObject.Find("GameController").GetComponent<GameController>().numberOfEnemies -= 1;
    }
}
