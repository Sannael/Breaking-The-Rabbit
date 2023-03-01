using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRanged : MonoBehaviour
{
    public GameObject projectile; //Recebe o objeto do projétil
    public Transform projectilePos; //Recebe o objeto que tem a posição do ponto de Spawn
    private GameObject player; //Recebe o Player
    private Animator slimeAnim; //Animator do slime
    private bool isVisible; //Checa se é visivel
    private float timer; //Temporizador para definir o intervalo de tempo do Spawn
    public float atkSpeedMin, atkSpeedMax; //ataque speed do inimigo, minimo e maximo, pra n atirar todos ao mesmo tempo;
    private float atkSpeed; //Velocidade de ataque do slime 
    public bool isAlive; //Checa se o slime ta vivo
    private bool direita; //Checa se ta a direita do alvo
    
    
    void Start()
    {
        direita = true;
        slimeAnim = this.GetComponent<Animator>();
        player = GameObject.Find("Player"); //Recebe o Player
        atkSpeed = Random.Range(atkSpeedMin, atkSpeedMax); //"Aleatoriza" a velocidade de ataque
        timer = atkSpeed - 1.5f; //Primeiro ataque é mais rapido
    }

    void Update()
    {
        isAlive = this.GetComponent<EnemyStatus>().isAlive; //Checa se o slime ta vivo
        if(isAlive == true)
        {
            if(player != null)
            {
                if(player.transform.position[0] < this.transform.position[0] && direita == true)
                {
                    direita = !direita;
                    transform.Rotate(0, 180, 0);
                }
                if(player.transform.position[0] > this.transform.position[0] && direita == false)
                {
                    direita = !direita;
                    transform.Rotate(0, 180, 0);
                }
            }
            
            //float distance = Vector2.Distance(transform.position,player.transform.position); //Verifica o valor da distância entre o inimigo e o player
            //Debug.Log(distance);

            if(player == null)
            {
                player = GameObject.Find("Player");
            }
            
            if (isVisible && player != null && player.GetComponent<PlayerScript>().isAlive == true)
            {
                timer += Time.deltaTime; //Gera um temporizador em segundos
                if (timer > atkSpeed) //A cada x segundos
                {
                    Animations("Shoot");
                    timer = 0; //Reseta o Timer
                }
            }
        }
    }

    public void Animations(string animation)
    {
        slimeAnim.Rebind();
        slimeAnim.SetTrigger(animation);
    }

    public void shoot()
    {
        Instantiate(projectile, projectilePos.position, Quaternion.identity); //Cria um projétil, conforme seu prefab
        timer = 0; //Reseta o Timer
        atkSpeed = Random.Range(atkSpeedMin, atkSpeedMax); //"Aleatoriza" a valocidade de ataque após cada tiro         
    }

    private void OnBecameVisible() //Checa se está visivel em alguma camera
    {
        isVisible = true;    
    }

    private void OnBecameInvisible() //Checa se está visivel em alguma camera 
    {
        isVisible = false;   
    }

}
