using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMelee : MonoBehaviour
{
    private GameObject player; //Var que vai receber o Player
    public float speed; //Determina a velocidade com que o inimigo irá se mover
    public bool isVisible; //checa se esta visivel em alguma camera
    public EnemyStatus enemyStastus; //Script de status do inimigo
    public bool isAlive; //Checa se o alvo ainda ta vivo; se pode realizar as tarefas normalmente 
    public DamageScript dmgScript; //Script de dano; pra caso precisar alterar
    
    void Start()
    {
        player = GameObject.Find("Player"); //Recebe o Player
        enemyStastus = this.GetComponent<EnemyStatus>(); //Puxa o status do inimigo; vida, armadura e etc
        dmgScript = this.GetComponent<DamageScript>(); //Puxa o script de dano
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
        isAlive = enemyStastus.isAlive;
        if(isAlive == true)
        {
            if(isVisible == true && player != null && player.GetComponent<PlayerScript>().isAlive == true) //Se tiver visivel ele segue o player
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime); //Move o inimigo em direção ao player 
            }
            else
            {
                transform.position = transform.position; //para de se movimentar 
            }
        
            if(player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
        
            //Usar caso o inimigo só for perseguir quando estiver a uma certa distância do jogador:
            //float distance = Vector2.Distance(transform.position, player.transform.position); //Verifica o valor da distância entre o inimigo e o player
        }
        else
        {
            this.GetComponent<DamageScript>().enabled = false;
        }
    }
    
    private void OnBecameVisible() //Quando o objeto se tornar visivel em qualquer camera do jogo (A do scene conta)
    {
        isVisible = true;
    }

    private void OnBecameInvisible()  //Quando o objeto não é visivel em nenhuma camera do jogo (A do scene conta)
    {
        isVisible = false;   
    }
}
