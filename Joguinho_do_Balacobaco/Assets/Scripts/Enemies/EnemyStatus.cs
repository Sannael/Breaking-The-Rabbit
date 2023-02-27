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
            Animations("Death");
            isAlive = false;  
        }

        if(canTakeDmg == false)
        {
            if(health >0)
            {
                StartCoroutine(Blink());
            }
            canTakeDmg = true;
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
        if(other.gameObject.tag == "PlayerWeapon" && canTakeDmg == true) //checa a tag doq trombou com ele, e se ta fora do tempo de invencibilidade
        {
            health = health - (other.gameObject.GetComponent<DamageScript>().damage - armor);//Pega o script de dano sa
            canTakeDmg = false;
        }
        if(other.gameObject.tag == "PlayerBullet" && canTakeDmg == true) //checa a tag doq trombou com ele, e se ta fora do tempo de invencibilidade
        {
            health = health - (other.gameObject.GetComponent<DamageScript>().damage - armor);//Pega o script de dano sa
            canTakeDmg = false;
        }
    }

    public IEnumerator Blink()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,100); 
        yield return new WaitForSeconds(0.30f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
        /*yield return new WaitForSeconds(0.30f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,100);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,100);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);*/
    }
    public IEnumerator ResetTakeDmg()
    {
        yield return new WaitForSeconds(0.10f);
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
