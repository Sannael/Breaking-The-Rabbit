using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameObject sceneManager; 
    public float speed; //Move Speed do cueio
    public Animator playerAnim;
    private bool direita; //checha a direção no eixo X
    private float direcao; //direção em float (<0 = esqerda; >0 = direita; 0 = parado)
    private bool walking, idle, roll; //actions
    public float rollCdr; //cooldown do rolamento
    private float rollCdrInitial; //armazena o valor inicial da variavel de cdr do roll (reset do valor de maneira pratica)
    public Rigidbody2D rb; //Corpo Rigido do player
    public bool canRoll; //verifica se pode rolar 
    public bool canMove; //verifica se pode se movimentar (Guima: deixei publico para acessar do script da câmera)
    public int health; //Vida do Cueio
    public Vector2 rbVelocity; //velocidade do rigidibody
    public GameObject gunCase; //Estojo de arma, é meio q a mão do player pra arma de fogo (talvez arma num geral (?) n pensei nisso) 
    public bool canTakeDamage; //Checa se o player pode tomar dano
    public bool isAlive; //Checa se ta vivo ou n //Evitar bugs
    public int armor; //Armadura do Cuelho
    public BoxCollider2D dmgCollider; //Box collider de gatilho pra tomar dano
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        armor = 0; 
        canMove = true;
        rollCdrInitial = rollCdr; //armazena o valor inicial da variavel de cdr do roll (reset do valor de maneira pratica)
        rbVelocity = rb.velocity; //armazena a velocidade inical do rigidbody2D  
    }

    void Update()
    {
        if(health <= 0 && isAlive == true) //se a vida zerar ele merre
        {
            DisbleAll(); //Função de desativar todos os itenbs do player
            isAlive = false; //Murreu :(
            Animations("Death"); //Animação de Death
        }
        else if(isAlive == true) //Se n tiver zerada pode fazer a farra
        {
            direcao = Input.GetAxis("Horizontal"); 
            
            if(canTakeDamage == true) //Se o playerpoder tomar dano
            {
                dmgCollider.enabled = true; //ativa o collider de dano
            }
            else
            {
                dmgCollider.enabled = false; //desativa o collider de dano
            }
        
            if((direcao >0 && direita == true) || (direcao <0 && direita == false)) //Checa se a necessidade de espelhar(Coelho olhar pra um lado e andar pro outro)
            {
                direita = !direita; //inverte o valor da direita (true pra false / false pra true)
                transform.Rotate(0f ,180 ,0f); //espelha a imagem
            }

            if(rollCdr >0) //Roll em cooldown
            {
                canRoll = false;
                rollCdr -= Time.deltaTime; //Diminui aos poucos o cdr da esquiva
            }
            else
            {
                canRoll = true; //Roll fora do cooldown
            }

            if(canMove == true)
            {
                Move();
            }
        
            walking = playerAnim.GetBool("Walking"); //Todo frame puxa valor igual o valor armazenado nos parametros do animator
            idle = playerAnim.GetBool("Idle");
            roll = playerAnim.GetBool("Roll");

            if(Input.GetKeyDown(KeyCode.LeftShift) && canRoll == true)  
            {
                StartCoroutine(Roll());
            }
        }
    }

    public void Animations(string animation) //Chama a animação que é passsada como parametro
    {
        playerAnim.Rebind();
        playerAnim.SetTrigger(animation);
    }
    
    public void DisbleAll() //Desativa todos os itens do player
    {
        gunCase.SetActive(false); //Desativa o coldre de arma, logo a arma
    }

    public void Death()
    {
        Destroy(gameObject);
        sceneManager.GetComponent<Manager>().LoadScene(0); //Ir para o Menu
    }

    void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0); //Eixos da movimentação (X, Y, Z)
        
        transform.position = transform.position + speed * move * Time.deltaTime; //Movimentação com mais fluidez do deltatime

        if(move[0] != 0 || move[1] !=0) //checa se o cueio ta em movimento
        {
            playerAnim.SetBool("Walking", true);
            playerAnim.SetBool("Idle", false);
        }
        else
        {
            playerAnim.SetBool("Walking", false);
            playerAnim.SetBool("Idle", true);
        }
    }

    public IEnumerator Roll()
    {
        canTakeDamage = false;
        gunCase.SetActive(false);
        rollCdr = 10; //Cdrzin aleatorio, só ignora (pra n correr risco de duplicar animação)
        canRoll = false;
        canMove = false;

        playerAnim.SetBool("Roll", true);
        playerAnim.SetBool("Walking", false);
        playerAnim.SetBool("Idle", false);

        float force = 5f; //Força gravitacional que é aplicada no momento do rolamento
        Vector2 vel = transform.right; //Valor aleatório só ignora tbm, pra n dar B.O depois aaaaaaaaaaaa :v (a var vel é usada pra força usada pro roll)
        vel[0] = transform.right[0] *  force; //Calculo da força no "Empurrao" pra acontecer o roll (Horizontal)

        if(Input.GetAxis("Vertical") > 0) //Checa se o player ta indo diagonal pra cima ou pra baixo 
        {
            vel[1] = transform.up[1] *  force; //Calculo da força no "Empurrao" pra acontecer o roll (Vertical) pra cima
        }
        else if(Input.GetAxis("Vertical") < 0) //Checa se o player ta indo diagonal pra cima ou pra baixo
        {
            vel[1] = transform.up[1] * (- force); //Calculo da força no "Empurrao" pra acontecer o roll (Vertical) pra baixo
        }
        else
        {
            vel[1] = 0f; //Sem empurrão pra cima ou baixo; empurrão reto na horizontal
        }
        rb.velocity = vel; //Altera o valor da velocity do rigidibody (tipo a força do empurro) aqui que a mágica acontece

        yield return new WaitForSeconds(0.6f); //espera o final da animação pra poder voltar ao normal
        playerAnim.SetBool("Roll", false);  
        rb.velocity = rbVelocity; //Reseta o valor da velocity do rigidibody
        canMove = true;
        rollCdr = rollCdrInitial; //Reseta o valor de Cdr do Roll
        gunCase.SetActive(true);
        canTakeDamage = true;
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyAtk")
        {            
            int damageTaken = other.gameObject.GetComponent<DamageScript>().damage;
            StartCoroutine(CanTakeDamage(damageTaken)); 
        }
    }

    public IEnumerator CanTakeDamage(int damageTaken) //Função de invencibilidade 
    {
        if(canTakeDamage == true && damageTaken > 0) //Se estiver fora do tempo de invencibilidade
        {
            canTakeDamage = false; //Seta pra ivulneravel
            TakeDamage(damageTaken); //Chama a função que diminui a vida (Faz pouca coisa agora mas depois vai ter armor e os karalho)
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,100); //Daqui pra baixo é pra ele piscar, basicamente deixo ele opaco e volto ao normal
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,100);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,100);
            yield return new WaitForSeconds(0.15f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255); // ^^^^^^^até aqui ^^^^^^^^
            canTakeDamage = true; //Fim do tempo de Invulnerabilidade
        }
        
    }

    public void TakeDamage(int damageTaken)
    {
        health =  health - (damageTaken - armor);
    }

}
