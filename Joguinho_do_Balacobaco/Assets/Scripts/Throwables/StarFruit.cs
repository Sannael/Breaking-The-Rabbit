using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFruit : MonoBehaviour
{
    public Item item;
    public float speed; //Velocidade da carambola
    public bool travel;
    private Animator anim;
    private int count; //pra n bugar e o player pegar duas carambola em uma
    private int ammoDropCount;  //pra não bugar e dropar duas munições   
    public GameObject ammo;
    private AmmoDrop ammoDropScript;
    private DamageScript dmgScript;
    private GameObject player;
    public CircleCollider2D rigidCol;
    public bool isVisible;
    [Header("Id das layers para funcionalidade do imã")]
    public int starfuitLayerId;
    public int coinLayerId;
    
    [Header("Sounds")]
    public AudioClip travelSound;
    private bool sound;
    [HideInInspector]
    public GameObject soundObj;
    private bool wall;
    void Start()
    {
        sound = true;
        gameObject.layer = starfuitLayerId;
        gameObject.tag = "StarFruit";
        isVisible = true;
        count = 1;
        ammoDropCount = 1;
        anim = this.GetComponent<Animator>();
        dmgScript = this.GetComponent<DamageScript>();
        ammoDropScript = ammo.GetComponent<AmmoDrop>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(wall == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
        }
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && sound == true)
        {
            soundObj = GameSounds.instance.CreateNewSoundLoop(travelSound);
            sound = false;
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Throw") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Throw2"))
        {
            Destroy(soundObj);
        }
        if(travel == true)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            anim.SetBool("Throw", true);
            
        }
        else
        {
            anim.SetBool("Throw", false);
            this.GetComponent<DamageScript>().enabled = false; //Desabilita o script de dano
            gameObject.layer = coinLayerId;
            gameObject.tag = "Coin";
        }
        if(isVisible == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.05f);//Carambola ir pro player aos poucos 
            transform.Rotate(0, 0, 20); //Carambola vai girandinho
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag != "Player" && other.tag != "PlayerBullet") //Se colidir com outro objeto, para o arremesso
        {
            travel = false;
            rigidCol.enabled = false;
            DropAmmo(other.gameObject);
        }
        if(other.tag == "Player" && travel == false && count >0) //Se tiver parado para o arremesso
        {
            PlayerTakeStarFruit();
        }
        if(other.tag == "Wall")
        {
            wall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Wall")
        {
            wall = false;
        }
    }

    public void DropAmmo(GameObject other)
    {
        if(other.gameObject.GetComponent<EnemyStatus>() != null && other.gameObject.GetComponent<EnemyStatus>().health >0)
        {
            if(other.gameObject.GetComponent<EnemyStatus>().health > dmgScript.trueDamage && ammoDropCount > 0) //Se a starfruit n matar o alvo
            {
                ammoDropCount --;
                GameObject ammoDrop = Instantiate(ammo, other.transform.position, other.transform.rotation);
                ammoDrop.GetComponent<AmmoDrop>().hitDrop = true;
                ammoDrop.GetComponent<Rigidbody2D>().velocity = transform.up * 3f; //Efeitinho da munição
            }
            if(other.gameObject.GetComponent<EnemyStatus>().health <= dmgScript.trueDamage && ammoDropCount > 0) //Se a starFruit matar o alvo
            {
                ammoDropCount --;
                GameObject ammoDrop = Instantiate(ammo, other.transform.position, other.transform.rotation);
                ammoDrop.GetComponent<AmmoDrop>().hitKill = true;
                ammoDrop.GetComponent<Rigidbody2D>().velocity = transform.up * 3f; //Efeitinho da munição
            }
        }
    }

    private void OnBecameInvisible() 
    {
        if(count > 0)
        {
            isVisible = false;
        }
        
    }

    private void PlayerTakeStarFruit()
    {
        count --;
        Destroy(gameObject);
        player.GetComponent<PlayerScript>().starFruitCount ++;
    }
}
