using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDash : MonoBehaviour
{
   private GameObject player; //Var que vai receber o Player
    public float speed; //Determina a velocidade com que o inimigo irá se mover
    public bool isVisible; //checa se esta visivel em alguma camera
    public EnemyStatus enemyStastus; //Script de status do inimigo
    public bool isAlive; //Checa se o alvo ainda ta vivo; se pode realizar as tarefas normalmente 
    public DamageScript dmgScript; //Script de dano; pra caso precisar alterar
    private bool direita;  //Checa se ta a direita do alvo
    public float atkSpeed; //Velocidade de ataque
    private float atkSpeedCurrent;
    private bool canAtk; //pode atacar (Dar o dash)
    private Vector3 playerAtkDistance; //Salva a distancia do player pra realizar um ataque
    private Animator slimeAnim; //Animator do slime
    private Rigidbody2D rb;
    private Vector2 rbVelocity; //Impulso do dash
    public float force; //Força do dash do slime
    private bool canMove; //Checa s epode mover
    void Start()
    {
        atkSpeedCurrent = atkSpeed; //Primeiro ataque não tem cooldown
        rb = this.GetComponent<Rigidbody2D>(); //Pega o Rigidbody do objeto
        rbVelocity = rb.velocity; //Armazena o valor inicial do velocity do rigidbody 
        slimeAnim = this.GetComponent<Animator>();
        direita = true; //Ele começa sempre olhando pra direita, mesmo se estiver a esquerda, ele se aejita solo
        player = GameObject.Find("Player"); //Recebe o Player
        enemyStastus = this.GetComponent<EnemyStatus>(); //Puxa o status do inimigo; vida, armadura e etc
        dmgScript = this.GetComponent<DamageScript>(); //Puxa o script de dano

        canMove = true;
        canAtk = true;
    }

    void Update()
    {
        isAlive = enemyStastus.isAlive;
        if(isAlive == true)
        {
            if(isVisible == true && player != null && player.GetComponent<PlayerScript>().isAlive == true && canMove == true) //Se tiver visivel ele segue o player
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime); //Move o inimigo em direção ao player

                if(player.transform.position[0] < transform.position[0] && direita == true) //Checa se precisa espelhar o Slime
                {
                direita = !direita; //Inverte a direção
                transform.Rotate(0, 180, 0); //Espelha o Slime 
                } 
                if(player.transform.position[0] > transform.position[0] && direita == false) //Checa se precisa espelhar o Slime
                {
                    direita = !direita; //Inverte a direção
                    transform.Rotate(0, 180, 0); //Espelha o Slime
                }

                atkSpeedCurrent += Time.deltaTime; //Reset de cooldown

                if(atkSpeedCurrent >= atkSpeed) //se for maior
                {
                    canAtk = true;
                }
                else
                {
                    canAtk = false;
                }
                

                if(Vector3.Distance(transform.position, player.transform.position) <= 3) //Checa se o player ta dentro da distancia de dash do slime
                {
                    playerAtkDistance = player.transform.position; //Armazena a posição do player
                    if(canAtk == true)
                    {
                        atkSpeedCurrent = 0; //Reseta o cooldown
                        animations("Dash");
                    }
                    
                }
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
            dmgScript.damage = 0; //Se morrer n da dano
        }
    }

    public void animations(string animation) //Chama a animação que é passada como parametro
    {
        slimeAnim.Rebind(); //Reseta os parametros do animator
        slimeAnim.SetTrigger(animation);
    }
    public void Dash()
    {
        playerAtkDistance = player.transform.position; //Armazena a posição do player antes do dash
        float distanceX = transform.position[0] - playerAtkDistance[0]; 
        float distanceY = transform.position[1] - playerAtkDistance[1]; //Calcula a distancia (eixo Y) do slime até o inimigo; <0 player acima; >0 player abaixo
        Vector2 vel = transform.position; //Valor random, se n taca algo buga ent ignoraaaaaaaa
        vel[0] = transform.right[0] * force; //Força de impulso pra direita
        
        /*if(distanceX < -0.35f)  
        {
            vel[0] = transform.right[0] * (force -2); 
        }
        else if(distanceY > 0.35f) 
        {
            vel[0] = transform.right[0] * (- (force -2)); 
        }
        else 
        {
            vel[0] = force; 
        }*/

        if(distanceY <-1) //se etrar aqui o Player ta acima do slime, logo dash pra diagonal-cima 
        {
            vel[1] = transform.up[1] * (force -2); //Calculo de impulso pra cima
        }
        else if(distanceY >1) //se etrar aqui o Player ta abaixo do slime, logo dash pra diagonal-baixo
        {
            vel[1] = transform.up[1] * (- (force -2)); //Mesmo calculo de impulso, sóq pra baixo :v
        }
        else 
        {
            vel[1] = 0f; //Se a disntacia do eixo Y for proxima de 0 ele da dash pra frente e n diagonal
        }
        rb.velocity = vel; //Aqui que o impulso acontece, ai rola o dash
        atkSpeedCurrent = 0; //Reseta o tempo de ataque

    }

    private void RestartMove() //Função pra voltar a se mover
    {
        canMove = true;
    }
    private void StopMove() //Função pra parar de mover
    {
        canMove = false;
        transform.position = transform.position;
    }

    private void EndOfDash() //Funçãozinha pro fim do dash
    {
        rb.velocity = rbVelocity; //Em resumo, quando acaba o dash, acaba o impulso
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
