using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health;
    public bool canTakeDmg = true; //Checa se pode tomar dano; pra tempo de invenc/ evitar dano duplicado
    public bool isAlive; //Checa se ta vivo; ajuda pra ajeitar animação
    public Animator slimeAnimator;


    void Start()
    {
        canTakeDmg = true;
        isAlive = true;
        slimeAnimator = this.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if(health <= 0 && isAlive == true) //Se a vida chegar a 0 é chamada a função de morte
        {
            Destroy(gameObject);
            //slimeAnimator.Rebind();
            //isAlive = false;
            //slimeAnimator.SetTrigger("Death");
            
        }
        if(isAlive == false)
        {
            Destroy(gameObject);
        }

        if(canTakeDmg == false)
        {
            canTakeDmg = true;
            //StartCoroutine(ResetTakeDmg()); //caso o inimigo tenha um tempo de invencibilidade pós ataque
        }
    }


    public void OnTriggerEnter2D (Collider2D other) //Função de colisão
    {
        if(other.gameObject.tag == "PlayerBullet" && canTakeDmg == true) //checa a tag doq trombou com ele, e se ta fora do tempo de invencibilidade
        {
            canTakeDmg = false;
            health = health - other.gameObject.GetComponent<DamageScript>().damage;//Pega o script de dano sa
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
        GameObject.Find("GameController").GetComponent<GameController>().numberOfEnemies += -1;
    }
}
