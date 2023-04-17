using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarrelGunScript : MonoBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;
    private Vector2 lookDirection; //Direção do mouse em relação a arma
    private float lookAngle; //Angulo do mouse em relação a arma
    public GameObject gun; //Gameobject da arma
    public bool direita = true; //Sentido da arma, pra espelhar a arte da arma
    [Tooltip("Pra arma não ficar estranha quando espelha tem q ter um valor do eixo Z que fiquei mais certinho na hora de espelhar. Só ir testtando pelo barrel até achar um bom valor")]
    public float zValueToMirror; //Valor que o eixo de rotação Z tem q ta pra não espelhar de maneira bugada
    void Start()
    {
        direita = true;
    }

    void FixedUpdate()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>()) - transform.position; //Armazena a direção do mouse
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * (Mathf.Rad2Deg); //Calculo maluco de angulo, não faço ideia de como funciona, só aceito
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle); //Rotaciona o braço pra mirar na direção do mouse  
        
        if(transform.rotation[2] > -zValueToMirror && transform.rotation[2] < zValueToMirror && direita == false) //Checa a necessidade de espelhar a arma (Mirando pra esquerda do Player)
        {
            direita = !direita; //Inverte o valor da direção
            gun.transform.Rotate(180f ,0f ,0f); //Espelha a arma
        }
        else if(transform.rotation[2] < -zValueToMirror && direita == true || transform.rotation[2] > zValueToMirror && direita == true)//Checa a necessidade de espelhar a arma (Mirando pra direita do Player)
        { 
            direita = !direita; //Inverte o valor da direção
            gun.transform.Rotate(180f ,0f ,0f); //Espelha a arma
        }
    }
}
