using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float duration, bulletSpeed; //Duração da bala até ser destruida / Velocidade da bala
    private bool travel = true; //Pra checar se a bala ta em moviento

    void Start()
    {   
        Destroy(gameObject, duration);      
    }

    void Update()
    {
        if(travel)
        {
            transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) //Checa se a collisão foi com o player
        {
            Destroy(gameObject); //Destroy o projétil
        }
    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }

}
