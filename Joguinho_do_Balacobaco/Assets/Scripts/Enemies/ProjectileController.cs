using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private GameObject player; //Var que vai receber o Player
    private Rigidbody2D rb; //Var que vai receber o o Rigid Body do projétil
    public float force; //Determina a força com que o projétil será lançado na direção do Player
    public float timer; //Temporizador para destruir o projétil
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Recebe o Rigid Body do projétil
        player = GameObject.Find("Player"); //Recebe o Player

        Vector3 direction = player.transform.position - transform.position; //Determina a direção em que o projétil será lançado
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force; //Determina a velocidade do projétil, baseada na direção e na força
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" || other.CompareTag("PlayerWeapon")) //Checa se a collisão foi com o player
        {
            Destroy(gameObject); //Destroy o projétil
        }
    }

    private void OnBecameInvisible() //Destrói o projetil quando sair da visao da camera (Sair da cena) 
    {
        Destroy(gameObject);
    }
}
