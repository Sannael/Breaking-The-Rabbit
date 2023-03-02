using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGold : MonoBehaviour
{
    private GameObject player;
    private bool isAlive;
    private EnemyStatus enemyStatus;
    public GameObject coin; //Muedinha
    private bool isVisible;
    public float speed;
    private Vector3 movePos;
    private GameObject saidaBaixo, saidaEsquerda, saidaDireita, saidaCima; //Todas as saidas, pra caso alguma sala n tiver todas as saidas e tal
    private int[] exits = new int[] {0, 0, 0, 0}; //Vetor que armazena qual saida que tem na sala atual e qual não tem
    private GameObject Roots; //Gameobject do Roots(Objeto que tem todas as saidas juntinhas)
    private int exitChoosen = 0; // Caso n tenha escolhido saida ainda, pra evitar repetições
    private int health;
    void Start()
    {
        isAlive = true;
        player = GameObject.Find("Player");
        enemyStatus = this.GetComponent<EnemyStatus>();
        health = enemyStatus.health;
        movePos = transform.position;
    }
    
    void Update()
    {
        isAlive = enemyStatus.isAlive;
        if(isAlive)
        {
            if(isVisible && Roots != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePos, speed * Time.deltaTime); //Pra movimentar na direção da saida
            }
            else
            {
                transform.position = transform.position;
            }
        }
        if(Roots == null)
        {
            Roots = GameObject.FindGameObjectWithTag("Roots"); //Acha o objeto que tem como herdeiro as paredes
            
        }
        else if(exitChoosen == 0)
        {
            FindExits();
        }

        if(health != enemyStatus.health)
        {
            DropCoin(); //Dropar moedas ao tomar dano
            health = enemyStatus.health;
        }
        
    }

    private void DropCoin()
    {
        int drop = Random.Range(1, 6); //Quantia aleatoria entre 1 e 5
        for(int i = 0; i < drop; i ++)
        {
            GameObject dropCoin = Instantiate(coin, transform.position, Quaternion.identity); //Criar um clone da moedinha
            dropCoin.GetComponent<Rigidbody2D>().velocity = transform.up * 2f; //Efeitinho da moedinha
        }
    }
    private void OnBecameVisible() //Quando o objeto se tornar visivel em qualquer camera do jogo (A do scene conta)
    {
        isVisible = true;
    }

    private void OnBecameInvisible()  //Quando o objeto não é visivel em nenhuma camera do jogo (A do scene conta)
    {
        isVisible = false; 
        Destroy(gameObject);  
    }

    private void FindExits()
    {
        exitChoosen ++;
        if(GameObject.Find("saida_ph") != null)
        {
            saidaBaixo = GameObject.Find("saida_ph"); //Armazena cada saida em sua determinada posição e direção
        }
        if(GameObject.Find("saida_ph (1)") != null)
        {
            saidaEsquerda = GameObject.Find("saida_ph (1)");
        }
        if(GameObject.Find("saida_ph (2)") != null)
        {
            saidaDireita = GameObject.Find("saida_ph (2)");
        }
        if(GameObject.Find("saida_ph (3)") != null)
        {
            saidaCima = GameObject.Find("saida_ph (3)");
        }
        
        ChooseOneExit(); //Função pra escolher de maneira aleatoria uma saida das que contém na fase
    }

    private void ChooseOneExit()
    {
        if(saidaBaixo != null) //Checa as saidas que tem na sala 
        {
            exits[0] = 1;
        }
        if(saidaEsquerda != null)
        { 
            exits[1] = 1;
        }
        if(saidaDireita != null)
        {
            exits[2] = 1;
        }
        if(saidaCima != null)
        {
            exits[3] = 1;
        }
        
        int exit = Random.Range(0 ,4); //Escolhe umam saida aleatoria

        if(exits[exit] > 0) //Checa se a saida escolhida existe, se existir vai até ela
        {
            switch(exit)
            {
                case 0:
                    movePos[1] = saidaBaixo.transform.position[1] - 20; //Se move pra até depois das paredes a mesma lógica pra todos, só uda direção
                    break;
                case 1:
                    movePos[0] = saidaEsquerda.transform.position[0] - 20;
                    break;
                case 2:
                    movePos[0] = saidaDireita.transform.position[0] + 20;
                    break;
                case 3:
                    movePos[1] = saidaCima.transform.position[1] + 20;
                    break;        
            }
        }
        else
        {
            ChooseOneExit(); //Se n tiver a saida escolhida refaz o processo
        }
    } 
}
