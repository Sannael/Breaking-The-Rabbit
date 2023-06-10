using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Reflection;

public class PlayerScript : MonoBehaviour
{
    public InputActionReference movement, mousePosition, meleeAtk, shoot, reload, dash, pause, openInventary, skipDialogue, starFruitAction, interaction;
    public InputActionReference[] hotbar = new InputActionReference[6];
    public PlayerInput playerInput;
    private GameObject sceneManager; 
    private Manager sceneManagerScript;
    public float speed; //Move Speed do cueio
    public Animator playerAnim;
    [SerializeField]
    private bool direita; //checha a direção no eixo X
    private float direcao; //direção em float (<0 = esquerda; >0 = direita; 0 = parado)
    private bool walking, idle, roll; //actions
    public float rollCdr; //cooldown do rolamento
    private float rollCdrInitial; //armazena o valor inicial da variavel de cdr do roll (reset do valor de maneira pratica)
    public Rigidbody2D rb; //Corpo Rigido do player
    public bool canRoll; //verifica se pode rolar 
    public bool canMove; //verifica se pode se movimentar (Guima: deixei publico para acessar do script da câmera)
    public int health; //Vida do Cueio
    public int maxHealth;
    public Vector2 rbVelocity; //velocidade do rigidibody
    public GameObject gunCase; //Estojo de arma, é meio q a mão do player pra arma de fogo (talvez arma num geral (?) n pensei nisso) 
    public GameObject melee;
    public bool canTakeDamage; //Checa se o player pode tomar dano
    public bool isAlive; //Checa se ta vivo ou n //Evitar bugs
    public int armor; //Armadura do Cuelho
    public float force; //Força gravitacional que é aplicada no momento do rolamento
    private bool stuned; //Checa se o player ta stunado
    public int coinCount; //Quantia de monys
    public int revolverAmmo, shotgunAmmo, pistolAmmo, assaultRifleAmmo, smgAmmo, magnumAmmo; //Tipos de munição que o player tem
    public GameObject starFruit;
    public int starFruitCount; //Contagem de carambolas
    public int starFruitMax;
    private GameController gameControllerScript; //script que controla quais teclas o player vai usar
    public GameObject pnlControls; //telinha de controles
    public GameObject pnlInventory;
    private bool canChangeGun = false; //Só é true quando tiver perto de uma arma no chão/loja/bau. Ao se afastar volta a ser false
    public GameObject newGun; //Arma que o player pode pegar, ficaria no chão/loja/bau
    private bool canChangeMelee;
    public GameObject newMelee;
    public bool confusion;
    public int extraLife;
    public PlayerStatus playerInitialStatus;
    public PlayerStatus playerStatusSave;
    public int shopDiscount;
    public GunSaveStatus initialGun;
    public MeleeSaveStatus initialMelee;
    private int room;
    private bool isRolling;

    [Header("Sounds")]
    public AudioClip rollSound;
    public AudioClip takingDamageSound;
    void Awake() 
    {
        playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
        pnlControls.GetComponent<ControlSettings>().Awake(); //Forçar os controles serem atualizados (caso houver alteração)
        pnlInventory.GetComponentInChildren<Inventory>().Awake(); //Força o inventário ser criado
        pnlInventory.GetComponentInChildren<CoreInventory>().Awake(); //Força a criação da instancia do inventario
        sceneManager = GameObject.Find("SceneManager");
        sceneManagerScript = sceneManager.GetComponent<Manager>();
        gameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();

        //Precisa de um if e else pra checar se é a primeira fase
        room = gameControllerScript.dungeon;
        if(room == 1)
        {
            initialGun.SetFirstGun();
            initialMelee.SetFirstMelee();
            TakeAllInitialStatus();
            ResetAllItensUsed();
        }   
        else
        {
            ReTakeStatus();
            CoreInventory._instance.inventory.ReTakeItensInfo();
        }
    }   

    private void ResetAllItensUsed()
    {
        Item[] allItens = Resources.LoadAll("Drops", typeof(Item)).Cast<Item>().ToArray();
        foreach(var item in allItens)
        {
            item.used = false;
        }
    }
    void Start()
    {
        isRolling = false;
        CoreInventory._instance.inventory.GetItem(starFruit.GetComponent<StarFruit>().item, 0, true, false, 3);
        canChangeGun = false;
        canChangeMelee = false;
        stuned = false;
        armor = 0; 
        canMove = true;
        rollCdrInitial = rollCdr; //armazena o valor inicial da variavel de cdr do roll (reset do valor de maneira pratica)
        rbVelocity = rb.velocity; //armazena a velocidade inical do rigidbody2D  
    }

    public void TakeAllInitialStatus()
    {
        playerInitialStatus.FillList();
        foreach(var status in playerInitialStatus.status)
        {
            ChangeVarValues(status.Key, status.Value, true);
        }
    } 
    public void ReTakeStatus()
    {
        playerStatusSave.FillList();
        foreach(var status in playerStatusSave.status)
        {
            ChangeVarValues(status.Key, status.Value, true);
        }
        GunSaveStatus gunsave = Resources.LoadAll("", typeof(GunSaveStatus)).Cast<GunSaveStatus>().First();
        gunsave.SetGun();
        MeleeSaveStatus meleeSave = Resources.LoadAll("", typeof(MeleeSaveStatus)).Cast<MeleeSaveStatus>().First();
        meleeSave.SetMelee();
    }

    public void ChangeDungeonFloor()
    {
        gunCase.GetComponentInChildren<GunStatus>().SaveGun();
        melee.GetComponentInChildren<MeleeController>().weapon.GetComponent<MeleeScript>().SaveMelee();
        FieldInfo[] scriptVars = typeof(PlayerScript).GetFields(BindingFlags.Public | BindingFlags.Instance);
        playerStatusSave.FillList();
        foreach(FieldInfo variable in scriptVars)
        {
            object varValue = variable.GetValue(this);
            string name = variable.Name;
            playerStatusSave.SaveList(name, varValue);
        }
        sceneManagerScript.LoadScene(5);
    } 

    void Update()
    {
        if(stuned == true)
        {
            EnableDisableAll(false,false);
        }
        if(gameControllerScript.isPaused == false)
        {
            if(health <= 0 && isAlive == true) //se a vida zerar ele merre
            {
                if(extraLife <=0)
                {
                    EnableDisableAll(false, false); //Função de desativar todos os itens do player
                    isAlive = false; //Murreu :(
                    Animations("Death"); //Animação de Death
                    Collider2D[] allColliders = this.GetComponents<Collider2D>();
                    foreach(var c in allColliders)
                    {
                        c.enabled = false;
                    }
                }
                else
                {
                    extraLife --;
                    //B.O pra resolver quando implementar item
                }
            }
            else if(isAlive == true && stuned == false) //Se n tiver zerada pode fazer a farra
            {
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

                if(dash.action.IsPressed()  && canRoll == true)  
                {
                    StartCoroutine(Roll());
                }

                if(canChangeGun == true && interaction.action.IsPressed()) //puxa o evento de trocar de arma
                {
                    canChangeGun = false;
                    newGun.GetComponent<ChangeGun>().ChangePlayerGun();
                }
                if(canChangeMelee == true && interaction.action.IsPressed())
                {
                    canChangeMelee = false;
                    newMelee.GetComponent<ChangeMelee>().changeMelee();
                }

            }
        }
    }


    public void Animations(string animation) //Chama a animação que é passsada como parametro
    {
        playerAnim.Rebind();
        playerAnim.SetTrigger(animation);
    }
    
    public void EnableDisableAll(bool gun, bool meleecase) //Desativa todos os itens do player
    {
        gunCase.SetActive(gun); //Desativa o coldre de arma, logo a arma
        melee.SetActive(meleecase);
    }

    public void playSoundEffect(AudioClip sound)
    {
        GameSounds.instance.PlaySingle(sound);
    }

    public void Death()
    {
        Destroy(gameObject);
        sceneManager.GetComponent<Manager>().LoadScene(0); //Ir para o Menu
    }

    void Move()
    {
        Vector3 move = movement.action.ReadValue<Vector3>();
        Vector2 dirMouse = Camera.main.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>());
        direcao = dirMouse[0] - transform.position[0];

        if(confusion == true)
        {
            rb.velocity = - move * speed;
        }
        else
        {
            rb.velocity = move * speed;
        }
        

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

    public void ChangeVarValues<value>(string statusName, value newValue, bool initialValues)
    {
        switch (statusName)
        {
            case "speed":
            if(initialValues == true)
            {
                speed = System.Convert.ToSingle(newValue);
            }
            else
            {
                speed += System.Convert.ToSingle(newValue);
            }
            
            break;
            
            case "rollCdr":
            if(initialValues == true)
            {
                rollCdr = System.Convert.ToSingle(newValue);
            }
            else
            {
                rollCdr += System.Convert.ToSingle(newValue);
            }
            
            break;

            case "health":
            if(initialValues == true)
            {
                health = System.Convert.ToInt32(newValue);
            }
            else
            {
                health += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "maxHealth":
            if(initialValues == true)
            {
                maxHealth = System.Convert.ToInt32(newValue);
            }
            else
            {
                maxHealth += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "isAlive":
            if(initialValues == true)
            {
                isAlive = System.Convert.ToBoolean(newValue);
            }
            else
            {
                isAlive = System.Convert.ToBoolean(newValue);
            }
            
            break;

            case "canTakeDamage":
            if(initialValues == true)
            {
                canTakeDamage = System.Convert.ToBoolean(newValue);
            }
            else
            {
                canTakeDamage = System.Convert.ToBoolean(newValue);
            }
            
            break;

            case "armor":
            if(initialValues == true)
            {
                armor = System.Convert.ToInt32(newValue);
            }
            else
            {
                armor += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "coinCount":
            if(initialValues == true)
            {
                coinCount = System.Convert.ToInt32(newValue);
            }
            else
            {
                coinCount += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "revolverAmmo":
            if(initialValues == true)
            {
                revolverAmmo = System.Convert.ToInt32(newValue);
            }
            else
            {
                revolverAmmo += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "shotgunAmmo":
            if(initialValues == true)
            {
                shotgunAmmo = System.Convert.ToInt32(newValue);
            }
            else
            {
                shotgunAmmo += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "pistolAmmo":
            if(initialValues == true)
            {
                pistolAmmo = System.Convert.ToInt32(newValue);
            }
            else
            {
                pistolAmmo += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "assaultRifleAmmo":
            if(initialValues == true)
            {
                assaultRifleAmmo = System.Convert.ToInt32(newValue);
            }
            else
            {
                assaultRifleAmmo += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "smgAmmo":
            if(initialValues == true)
            {
                smgAmmo = System.Convert.ToInt32(newValue);
            }
            else
            {
                smgAmmo += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "magnumAmmo":
            if(initialValues == true)
            {
                magnumAmmo = System.Convert.ToInt32(newValue);
            }
            else
            {
                magnumAmmo += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "starFruitCount":
            if(initialValues == true)
            {
                starFruitCount = System.Convert.ToInt32(newValue);
            }
            else
            {
                starFruitCount += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "starFruitMax":
            if(initialValues == true)
            {
                starFruitMax = System.Convert.ToInt32(newValue);
            }
            else
            {
                starFruitMax += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "confusion":
            if(initialValues == true)
            {
                confusion = System.Convert.ToBoolean(newValue);
            }
            else
            {
                confusion = System.Convert.ToBoolean(newValue);
            }
            
            break;

            case "extraLife":
            if(initialValues == true)
            {
                extraLife = System.Convert.ToInt32(newValue);
            }
            else
            {
                extraLife += System.Convert.ToInt32(newValue);
            }
            
            break;

            case "stunTime":
            Stun(System.Convert.ToSingle(newValue));
            break;

            case "shopDiscount":
            shopDiscount = System.Convert.ToInt32(newValue);
            break;
        }
    }

    public IEnumerator Roll()
    {
        canTakeDamage = false;
        EnableDisableAll(false,false);
        rollCdr = 10; //Cdrzin aleatorio, só ignora (pra n correr risco de duplicar animação)
        canRoll = false;
        canMove = false;

        playerAnim.SetBool("Roll", true);
        playerAnim.SetBool("Walking", false);
        playerAnim.SetBool("Idle", false);

        
        Vector2 vel = transform.right; //Valor aleatório só ignora tbm, pra n dar B.O depois aaaaaaaaaaaa :v (a var vel é usada pra força usada pro roll)
        Vector3 move = movement.action.ReadValue<Vector3>();
        bool rotate = false;

        if(move[0] >0 && direita == true) //Checa a direção que o player ta olhando e a direção que ta andando, pra ver se precisa espelhar
        { //Player na direita (olhando pra esquerda) e andando pra direita (andando de costas); precisa espelhar
            transform.Rotate(0, 180,0);
            rotate = true;
        }
        if(move[0] <0 && direita == false)
        {//Player na esquerda (olhando pra direita) e andando pra esquerda (andando de costas); precisa espelhar
            transform.Rotate(0, 180,0);
            rotate = true;
        }
        vel[0] = transform.right[0] *  force;//Calculo da força no "Empurrao" pra acontecer o roll (Horizontal)

        if(move[1] > 0) //Checa se o player ta indo diagonal pra cima ou pra baixo 
        {
            vel[1] = transform.up[1] *  force; //Calculo da força no "Empurrao" pra acontecer o roll (Vertical) pra cima
        }
        else if(move[1] < 0) //Checa se o player ta indo diagonal pra cima ou pra baixo
        {
            vel[1] = transform.up[1] * (- force); //Calculo da força no "Empurrao" pra acontecer o roll (Vertical) pra baixo
        }
        else
        {
            vel[1] = 0f; //Sem empurrão pra cima ou baixo; empurrão reto na horizontal
        }
        isRolling = true;
        GameSounds.instance.CreateNewSound(rollSound);
        rb.velocity = vel; //Altera o valor da velocity do rigidibody (tipo a força do empurro) aqui que a mágica acontece
    
        yield return new WaitForSeconds(0.6f); //espera o final da animação pra poder voltar ao normal
        playerAnim.SetBool("Roll", false);  
        rb.velocity = rbVelocity; //Reseta o valor da velocity do rigidibody
        canMove = true;
        rollCdr = rollCdrInitial; //Reseta o valor de Cdr do Roll
        gunCase.GetComponentInChildren<GunStatus>().reloading = false; //Se n fizer isso, caso usar o rolamento recarregando da pau
        EnableDisableAll(true,true);
        canTakeDamage = true;
        isRolling = false;
        
        if(rotate == true) //Se espelhou quando acabar o roll tem q espelhar dnovo pra voltar ao normal
        {
            transform.Rotate(0,-180,0);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyAtk" || other.gameObject.tag == "BossStage" || other.gameObject.tag == "BossStageAtk") //Se for atingido por um inimigo ou um atk dele (projetil, aremessavel e os karalho)
        {
            if(other.GetComponent<DamageScript>() != null && other.GetComponent<DamageScript>().isActiveAndEnabled) //Se o que trombar no player der dano, preciso saber quanto
            {      
                int damageTaken = other.gameObject.GetComponent<DamageScript>().damage; //Dano normal; armaruda e vida
                int trueDamage = other.gameObject.GetComponent<DamageScript>().trueDamage; //Dano verdadeiro; direto na vida ignora toda e qualquer armadura
                StartCoroutine(CanTakeDamage(damageTaken, trueDamage)); //Chamo função de tomar dano/ ficar invulneravel 
            }
            if(other.GetComponent<StunScript>() != null && other.GetComponent<StunScript>().isActiveAndEnabled) //Checa se oq trombou com o player tem stun e se ta ativo no momento da trombada
            {
                float time = other.GetComponent<StunScript>().stunTime;
                StartCoroutine(Stun(time)); //Funçãozinha de tomar stun
            }
        }

        if(other.gameObject.tag == "ChangeGun") //Quando entrar na area de troca de arma 
        {
            canChangeGun = true;   
            newGun = other.gameObject;
            other.GetComponent<ChangeGun>().canChange = true;
        }
        if(other.gameObject.tag == "ChangeMelee")
        {
            canChangeMelee = true;
            newMelee = other.gameObject;
            other.GetComponent<ChangeMelee>().canChange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "ChangeGun") //Quando sair na area de troca de arma 
        {
            other.GetComponent<ChangeGun>().canChange = false;
            canChangeGun = false;
        }
        if(other.gameObject.tag == "ChangeMelee")
        {
            other.GetComponent<ChangeMelee>().canChange = false;
            canChangeMelee = false;
        }
    }

    public void TakeCoin(int value)
    {
        coinCount += value;
    }


    public IEnumerator CanTakeDamage(int damageTaken, int trueDamage) //Função de invencibilidade 
    {
        if(canTakeDamage == true) //Se estiver fora do tempo de invencibilidade
        {
            canTakeDamage = false; //Seta pra ivulneravel
            TakeDamage(damageTaken, trueDamage); //Chama a função que diminui a vida (Faz pouca coisa agora mas depois vai ter armor e os karalho)
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

    public void TakeDamage(int damageTaken, int trueDamage)
    {
        //GameSounds.instance.PlaySingle(takingDamageSound);
        GameSounds.instance.CreateNewSound(takingDamageSound);
        int armorDefend = Random.Range(0, 21);
        if(armorDefend <= armor)
        {
            if((damageTaken - armor) > 0)
            {
                health =  health - (damageTaken - armor); //Calculo de dano, contando com a armadura
            }
        }
        else
        {
            health = health - damageTaken;
        }
        health =  health - trueDamage;
    }

    public IEnumerator Stun(float stunTime)
    {
        if(isRolling == false)
        {
            stuned = true;
            //gunCase.SetActive(false); //Desativa a arma, pra quando tiver stunado n atira 
            EnableDisableAll(false, false);
            playerAnim.SetBool("Stun", true);
            yield return new WaitForSeconds(stunTime); //Stuna durante o tempo certin
            playerAnim.SetBool("Stun", false);
            EnableDisableAll(true, true);
            //gunCase.SetActive(true); //Ativa a arma
            stuned = false;
        }
    }
    public void TakeAmmo(string ammoType, int ammoCount) //recupera a munição que esta na arma (pega o tipo da arma e a quantia de munição, qnd trocar de arma por exemplo)
    {
        switch(ammoType)
        {
            case "Pistol":
            pistolAmmo += ammoCount;
            break;

            case "Revolver":
            revolverAmmo += ammoCount;
            break;

            case "Shotgun":
            shotgunAmmo += ammoCount;
            break;

            case "AssaultRifle":
            assaultRifleAmmo += ammoCount;
            break;

            case "SMG":
            smgAmmo += ammoCount;
            break;

            case "Magnum":
            magnumAmmo += ammoCount;
            break;
        }
    }
}
