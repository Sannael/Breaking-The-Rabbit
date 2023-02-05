using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRanged : MonoBehaviour
{
    public GameObject projectile; //Recebe o objeto do projétil
    public Transform projectilePos; //Recebe o objeto que tem a posição do ponto de Spawn
    private GameObject player; //Recebe o Player
    private bool isVisible; //Checa se é visivel

    private float timer; //Temporizador para definir o intervalo de tempo do Spawn
    
    void Start()
    {
        player = GameObject.Find("Player"); //Recebe o Player
    }

    void Update()
    {
        //float distance = Vector2.Distance(transform.position,player.transform.position); //Verifica o valor da distância entre o inimigo e o player
        //Debug.Log(distance);

        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        
        if (isVisible && player != null && player.GetComponent<PlayerScript>().isAlive == true)
        {
            timer += Time.deltaTime; //Gera um temporizador em segundos
            if (timer > 1) //A cada x segundos
            {
                timer = 0; //Reseta o Timer
                shoot(); //Chama o método de atirar o projétil
            }
        }
    }

    public void shoot()
    {
        Instantiate(projectile, projectilePos.position, Quaternion.identity); //Cria um projétil, conforme seu prefab
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
