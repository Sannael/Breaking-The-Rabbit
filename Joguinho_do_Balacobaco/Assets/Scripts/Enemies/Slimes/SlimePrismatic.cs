using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePrismatic : MonoBehaviour
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
    public float distaceInSquares; //Distancia do dash em quadrados (metade da scale) 
    private StunScript stunScript; //Script de Stun
    public GameObject projectile; //Recebe o objeto do projétil
    public Transform projectilePos; //Recebe o objeto que tem a posição do ponto de Spawnn
    public float atkSpeedMin, atkSpeedMax; //ataque speed do inimigo, minimo e maximo, pra n atirar todos ao mesmo tempo
    private bool canShoot;
    private Color32[] colors = new Color32[7]; //Possibilidades de cores que o slime pode se transformar
    private bool effect; //Checa se tem algum efeito ativado
    public GameObject[] slimes; //Todos os slimes que o prismatico pode spawnar após morrer
    public GameObject slimeGold;
    private int health;
    void Start()
    {
        stunScript = this.GetComponent<StunScript>();
        dmgScript = this.GetComponent<DamageScript>();
        DisableAllEffects();

        atkSpeed = Random.Range(atkSpeedMin, atkSpeedMax);
        atkSpeedCurrent = atkSpeed; //Primeiro ataque não tem cooldown
        rb = this.GetComponent<Rigidbody2D>(); //Pega o Rigidbody do objeto
        rbVelocity = rb.velocity; //Armazena o valor inicial do velocity do rigidbody 
        slimeAnim = this.GetComponent<Animator>();
        direita = true; //Ele começa sempre olhando pra direita, mesmo se estiver a esquerda, ele se ajeita solo
        player = GameObject.Find("Player"); //Recebe o Player
        enemyStastus = this.GetComponent<EnemyStatus>(); //Puxa o status do inimigo; vida, armadura e etc
        canMove = true;
        canAtk = true;
        canShoot = true;
        health = enemyStastus.health;

        SetColor(); //Função que atribui as cores ao Vetor das cores k
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
                    canShoot = true;
                }
                else
                {
                    canShoot = false;
                }
                
                if(Vector3.Distance(transform.position, player.transform.position) <= distaceInSquares) //Checa se o player ta dentro da distancia de dash do slime
                {
                    playerAtkDistance = player.transform.position; //Armazena a posição do player
                    if(canAtk == true)
                    {
                        animations("Dash");
                        canShoot = false;
                    }
                }
                else
                {
                    if(canShoot == true)
                    {
                        animations("Shoot");
                        canAtk = false;
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

            if(Vector3.Distance(transform.position, player.transform.position) < 3.5f) //Quando tiver a uma distancia menor que 3,5 quadrados de distancia do player
            {
                if(effect == false) //Se não tiver com nenhum efeito ativado, ativar algum
                {
                    RandomEffect();
                }  
            }
            else //Quando sai dessa distancia desativa todos os efeitos, pra poder deixar randomico de novo
            {
                DisableAllEffects();    
            }
        //Usar caso o inimigo só for perseguir quando estiver a uma certa distância do jogador:
        //float distance = Vector2.Distance(transform.position, player.transform.position); //Verifica o valor da distância entre o inimigo e o player
        }
        else //Se morrer desabilita todos os efeitos; Evita causar dano após a morte
        {
            DisableAllEffects();
        }
    } 
    private void SpawnSlimes()
    {
        for(int i =0; i <5; i ++) //Quando morre spawna slimes
        {
            int randSpawn = Random.Range(0, slimes.Length);
            Instantiate(slimes[randSpawn], transform.position, Quaternion.identity);
        }
        float goldChance = Random.Range(0,10); //Chance de spawnar 1 slime gold
        if(goldChance <= 1.5f)
        {
            Instantiate(slimeGold, transform.position, Quaternion.identity);
        }
    }

    public void animations(string animation) //Chama a animação que é passada como parametro
    {
        slimeAnim.Rebind(); //Reseta os parametros do animator
        slimeAnim.SetTrigger(animation);
    }
    public void Shoot()
    {
        Instantiate(projectile, projectilePos.position, Quaternion.identity); //Cria um projétil, conforme seu prefab
        atkSpeedCurrent = 0; //Reseta o Timer
        atkSpeed = Random.Range(atkSpeedMin, atkSpeedMax); //"Aleatoriza" a valocidade de ataque após cada tiro
        canAtk = true;         
    }
    public void Dash()
    {
        playerAtkDistance = player.transform.position; //Armazena a posição do player antes do dash
        float distanceX = transform.position[0] - playerAtkDistance[0]; //Calcula a distancia (eixo X) do slime até o player; entre -0,35 e 0,35 player bem em cima ou baixo
        float distanceY = transform.position[1] - playerAtkDistance[1]; //Calcula a distancia (eixo Y) do slime até o player; <0.4 player acima; >0.4 player abaixo
        Vector2 vel = transform.position; //Valor random, se n taca algo buga ent ignoraaaaaaaa
        
        if((distanceX > -0.35 && distanceX < 0.35)) //Eixo X dentro dessa distancia quer dizer que o player e o slime estão na mesma reta na horizontal
        {
            force += 1; //Aplica mais força pra cima/baixo caso tiver em linha reta
            vel[0] = 0f; //Sem força pra direita  
        }
        else
        {
            vel[0] = transform.right[0] * force; //Em qualquer outra dsitancia, a força do dash é normal
        }

        if(distanceY <-0.4f) //se etrar aqui o Player ta acima do slime, logo dash pra diagonal-cima 
        {
            vel[1] = transform.up[1] * (force -2); //Calculo de impulso pra cima
        }
        else if(distanceY >0.4f) //se etrar aqui o Player ta abaixo do slime, logo dash pra diagonal-baixo
        {
            vel[1] = transform.up[1] * (- (force -2)); //Mesmo calculo de impulso, sóq pra baixo :v
        }
        else 
        {
            vel[1] = 0f; //Se a distancia do eixo Y for proxima de 0 ele da dash pra frente e n diagonal
        }
        rb.velocity = vel; //Aqui que o impulso acontece, ai rola o dash
    }

    private void RestartMove() //Função pra voltar a se mover
    {
        canMove = true;
        canAtk = true;
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
    }
    
    private void OnBecameVisible() //Quando o objeto se tornar visivel em qualquer camera do jogo (A do scene conta)
    {
        isVisible = true;
    }

    private void OnBecameInvisible()  //Quando o objeto não é visivel em nenhuma camera do jogo (A do scene conta)
    {
        isVisible = false;   
    }

    private IEnumerator Prisma()
    {
        for(int color = 0; color < colors.Length; color ++)
        {
            Color32 col = this.gameObject.GetComponent<SpriteRenderer>().color; //Armazena a cor atual do slime
            for(float i = 0; i < 1; i += 0.01f) //Loop que se repete 10 vezes, aumentando o Tom da cor
            {
                if(health != enemyStastus.health) //Se tomar dano, para o Coroutine pra ele podes piscar sem bugar a cor
                {
                    StartCoroutine(enemyStastus.Blink()); //Chama o coroutine de piscar e depois volta ao normal
                    yield return new WaitForSeconds(0.30f);
                    health = enemyStastus.health; //Reseta o valor da vida 
                }
                yield return null;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color32.Lerp(col, colors[color], i); //Efeitinho pra mudar entre a cor atural e a nova cor de uma maneira mais "natural"
            } 
        }
        StartCoroutine(Prisma()); //Chamar o Coroutine denovo (Loop eterno)
    }  
    private void SetColor() //somente armazena as cores possiveis em posições dos vetores
    {
        colors[0] = new Color32(255, 0, 0, 255);  //Vermelho
        colors[1] = new Color32(255, 117, 24, 255); //Laranja
        colors[2] = new Color32(255, 255, 0, 255); //Amarelo
        colors[3] = new Color32(50, 205, 50, 255); //Verde
        colors[4] = new Color32(0, 255, 255, 255); //Azul claro
        colors[5] = new Color32(65, 105, 255, 255); //Royal blue 
        colors[6] = new Color32(75, 0 , 130, 255); //Indigo

        StartCoroutine(Prisma());
    }

    private void DisableAllEffects() //Desabilita todos os efeitos 
    {
        effect = false;
        dmgScript.enabled = false;
        stunScript.enabled = false;
    }
    private void RandomEffect() //Troca os efeitos que o slime vai inflingir
    {
        effect = true;
        int effectId = Random.Range(0, 2); //escolhe um dos efeitos dentre a quantia de efeitos que o slime tem

        switch(effectId) //Ativa o efeito selecionado 
        {
            case 0:
                this.gameObject.GetComponent<DamageScript>().enabled = true;
                break;
            case 1:
                this.gameObject.GetComponent<StunScript>().enabled = true;
                break;
        } 
    }
}
