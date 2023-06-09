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
    [Tooltip("Distancia que o player tem que chegar pro slime dar o dash, calculada em quadrados, ou escale /2")]
    public float distaceInSquares; //Distancia dpo dash em quadrados (metade da scale) 
    private StunScript stunScript; //Script de Stun
    public CapsuleCollider2D capColl; //Colisor de stun; ativa só no dash se n pode bugar, colisor sempre da dor de cabeça ;-;
    public float dashCoefficient; //Variavel que divide a força do dash, caso o player tiver mt longe, n fca um dash mt forte
    [Header("Sounds")]
    public AudioClip dashSound;
    void Start()
    {
        if(this.GetComponent<StunScript>() != null) //Se o Slime tiver script de stun armazena ele 
        {
            stunScript = this.GetComponent<StunScript>();
        }
        if(this.GetComponent<DamageScript>() != null) //Se o Slime tiver script de dano armazena ele 
        {
            dmgScript = this.GetComponent<DamageScript>(); //Puxa o script de dano
        }
        atkSpeedCurrent = atkSpeed; //Primeiro ataque não tem cooldown
        rb = this.GetComponent<Rigidbody2D>(); //Pega o Rigidbody do objeto
        rbVelocity = rb.velocity; //Armazena o valor inicial do velocity do rigidbody 
        slimeAnim = this.GetComponent<Animator>();
        direita = true; //Ele começa sempre olhando pra direita, mesmo se estiver a esquerda, ele se aejita solo
        player = GameObject.Find("Player"); //Recebe o Player
        enemyStastus = this.GetComponent<EnemyStatus>(); //Puxa o status do inimigo; vida, armadura e etc
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
                

                if(Vector3.Distance(transform.position, player.transform.position) <= distaceInSquares) //Checa se o player ta dentro da distancia de dash do slime
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
            if(dmgScript != null)
            {
                dmgScript.enabled = false; //Se morrer n da dano
            }
            if(stunScript != null)
            {
                stunScript.enabled = false; //Se morrer n stuna (né não?)
            }
        }
    }

    public void animations(string animation) //Chama a animação que é passada como parametro
    {
        slimeAnim.Rebind(); //Reseta os parametros do animator
        slimeAnim.SetTrigger(animation);
    }
    public void Dash()
    {
        rb.mass = 100;
        if(stunScript != null) 
        {
            stunScript.enabled = true; //Habilita o stun  durante o dash
        }
        capColl.enabled = true;
        Vector3 mov = transform.position - player.transform.position; //Pego a distancia do player até o inimigo
        Vector2 tmov = new Vector2(mov[0], mov[1]); //puxo ela em um vector2, pra n dar merda e n usamos o Z
        tmov *= force;
        if(tmov[1] > 11 || tmov[1] < -11) // se for mt longe pra cima ele fica torto, ent faço esse role ai q da bom
        {
            tmov[1] /= (dashCoefficient * 0.9f);
        }
        GameSounds.instance.CreateNewSound(dashSound);
        
        rb.velocity = - (tmov); //Dashzin
        atkSpeedCurrent = 0; //Reseta o tempo de ataque
        rb.mass = 500;

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
        if(stunScript != null) 
        {
            stunScript.enabled = false; //Desabilita o stun quando acabar o dash
        }   
        rb.velocity = rbVelocity; //Em resumo, quando acaba o dash, acaba o impulso
        rb.mass = 500;
        capColl.enabled = false;
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
