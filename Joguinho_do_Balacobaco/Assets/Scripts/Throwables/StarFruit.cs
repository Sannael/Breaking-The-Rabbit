using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFruit : MonoBehaviour
{
    public float speed; //Velocidade da carambola
    public bool travel;
    private Animator anim;
    private int count; //pra n bugar e o player pegar duas carambola em uma
    private int ammoDropCount;  //pra não bugar e dropar duas munições   
    public GameObject ammo;
    private AmmoDrop ammoDropScript;
    private DamageScript dmgScript;
    void Start()
    {
        count = 1;
        ammoDropCount = 1;
        anim = this.GetComponent<Animator>();
        dmgScript = this.GetComponent<DamageScript>();
        ammoDropScript = ammo.GetComponent<AmmoDrop>();
    }

    // Update is called once per frame
    void Update()
    {
        if(travel == true)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            anim.SetBool("Throw", true);
        }
        else
        {
            anim.SetBool("Throw", false);
            dmgScript.enabled = false; //Desabilita o script de dano
        }
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag != "Player" && other.tag != "PlayerBullet") //Se colidir com outro objeto, para o arremesso
        {
            travel = false;
            DropAmmo(other.gameObject);
        }
        if(other.tag == "Player" && travel == false && count >0) //Se tiver parado para o arremesso
        {
            other.GetComponent<PlayerScript>().starFruitCount ++;
            count --;
            Destroy(gameObject);
        }
    }

    public void DropAmmo(GameObject other)
    {
        if(other.gameObject.GetComponent<EnemyStatus>() != null)
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
}
