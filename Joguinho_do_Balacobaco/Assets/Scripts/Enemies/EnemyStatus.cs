using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health;
    public bool canTakeDmg = true; //Checa se pode tomar dano; pra tempo de invenc/ evitar dano duplicado
    public bool isAlive; //Checa se ta vivo; ajuda pra ajeitar animação
    private Animator slimeAnimator;
    public int armor; //armadura do inimigo
    private bool sword; //Checa se tomou dano por uma arma melee, evita bugs de dano dobrado
    void Start()
    {
        sword = false;
        canTakeDmg = true;
        isAlive = true;
        slimeAnimator = this.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if(health <= 0 && isAlive == true) //Se a vida chegar a 0 é chamada a função de morte
        {
            Animations("Death");
            isAlive = false;  
        }

        if(canTakeDmg == false)
        {
            if(health >0)
            {
                StartCoroutine(Blink());
            }
            //canTakeDmg = true;
            //StartCoroutine(ResetTakeDmg()); //caso o inimigo tenha um tempo de invencibilidade pós ataque
        }
    }

    public void Animations(string animation)
    {
        slimeAnimator.Rebind();
        slimeAnimator.SetTrigger(animation);
    }
    public void Death()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D (Collider2D other) //Função de colisão
    {
        if(other.gameObject.tag == "PlayerWeapon" && canTakeDmg == true && sword == false) //checa a tag doq trombou com ele, e se ta fora do tempo de invencibilidade
        {
            sword = true;
            canTakeDmg = false;
            int damageTaken = other.gameObject.GetComponent<DamageScript>().damage; //Dano normal; armaruda e vida
            int trueDamage = other.gameObject.GetComponent<DamageScript>().trueDamage; //Dano verdadeiro; direto na vida ignora toda e qualquer armadura
            TakeDamage(damageTaken, trueDamage);
        }
        if(other.gameObject.tag == "PlayerBullet" && canTakeDmg == true) //checa a tag doq trombou com ele, e se ta fora do tempo de invencibilidade
        {
            int damageTaken = other.gameObject.GetComponent<DamageScript>().damage; //Dano normal; armaruda e vida
            int trueDamage = other.gameObject.GetComponent<DamageScript>().trueDamage; //Dano verdadeiro; direto na vida ignora toda e qualquer armadura
            canTakeDmg = false;
            TakeDamage(damageTaken, trueDamage);
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "StarFruit" && canTakeDmg == true) //checa a tag doq trombou com ele, e se ta fora do tempo de invencibilidade
        {
            if(other.gameObject.GetComponent<DamageScript>().isActiveAndEnabled)
            {
                int damageTaken = other.gameObject.GetComponent<DamageScript>().damage; //Dano normal; armaruda e vida
                int trueDamage = other.gameObject.GetComponent<DamageScript>().trueDamage; //Dano verdadeiro; direto na vida ignora toda e qualquer armadura
                canTakeDmg = false;
                TakeDamage(damageTaken, trueDamage);
            }
        }
    }

    private void TakeDamage(int damageTaken, int trueDamage)
    {
        if((damageTaken - armor) > 0)
        {
            health =  health - (damageTaken - armor); //Calculo de dano, contando com a armadura
        }
        health =  health - trueDamage;

        StartCoroutine(ResetTakeDmg());
    }

    public IEnumerator Blink()
    {
        Color32 colorNow = this.gameObject.GetComponent<SpriteRenderer>().color; //Armazenar a cor do Inimigo,pra quando piscar n fica branco ou outra cor random
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(colorNow[0] ,colorNow[1], colorNow[2], 100); 
        yield return new WaitForSeconds(0.30f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(colorNow[0] ,colorNow[1], colorNow[2], 255);
    }
    public IEnumerator ResetTakeDmg()
    {
        if(sword == true) //Dano pra espada demora um tequin pra poder tomar dano dde novo, evita tomar o dano 2 vezes ou mais no mesmo ataque, hitbox é foda
        {
            yield return new WaitForSeconds(0.4f);
            sword = false;
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
        canTakeDmg = true;
    }

    private void OnBecameVisible() //Quando o objeto se tornar visivel em qualquer camera do jogo (A do scene conta)
    {
        GameObject.Find("GameController").GetComponent<GameController>().numberOfEnemies += 1;
    }

    private void OnDestroy()
    {
        if(GameObject.Find("GameController") != null)
        {
            GameObject.Find("GameController").GetComponent<GameController>().numberOfEnemies += -1;
        }
    }
}
