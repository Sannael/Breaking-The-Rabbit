using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGunScript : MonoBehaviour
{
    private Vector2 lookDirection; //Direção do mouse em relação a arma
    private float lookAngle; //Angulo do mouse em relação a arma
    public GameObject gun; //Gameobject da arma
    private bool direita = true; //Sentido da arma, pra espelhar a arte da arma
    void Start()
    {
    }

    void FixedUpdate()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //Armazena a direção do mouse 
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * (Mathf.Rad2Deg); //Calculo maluco de angulo, não faço ideia dde como funciona, só aceito
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle); //Rotaciona o braço pra mirar na direção do mouse  
        
        if(transform.rotation[2] > -0.7f && transform.rotation[2] < 0.7f && direita == false) //Checa a necessidade de espelhar a arma (Mirando pra esquerda do Player)
        {
            direita = !direita; //Inverte o valor da direção
            gun.transform.Rotate(180f ,0f ,0f); //Espelha a arma
        }
        else if(transform.rotation[2] < -0.7f && direita == true || transform.rotation[2] > 0.7f && direita == true)//Checa a necessidade de espelhar a arma (Mirando pra direita do Player)
        { 
            direita = !direita; //Inverte o valor da direção
            gun.transform.Rotate(180f ,0f ,0f); //Espelha a arma
        }
    }
}
