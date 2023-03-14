using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotSpriteControl : MonoBehaviour
{
    public Sprite[] sprite; //sprites da cenoura
    public int spriteChange; //Id pra mudar a sprite
    public int carrotId; //Id da carrot
    private int health; //Vida do Player
    public GameObject lifeManager; //Obj onde fica as barrinha de vida
    
    void Start()
    {
        lifeManager = GameObject.Find("Life");
    }

    // Update is called once per frame
    void Update()
    {
        health = GameObject.Find("Player").GetComponent<PlayerScript>().health;
        if(health - (carrotId *2) > -1) //Se om resto de divisão for: > -1: manter o sprite da ultima cenoura como cenoura inteira
        {
            TrocaSprite(1);
        }
        else if(health - (carrotId *2) == -1) //Se for = -1: Mudar sprite da ultima cenoura pra cenoura "Mordida"
        {
            TrocaSprite(0);
        }
        else //se for < -1: Destroy a ultima cenoura
        {
            Destroy();
        }
        
    }

    public void TrocaSprite(int Id)
    {
        if (Id == 1)
        {
            gameObject.GetComponent<Image>().sprite = sprite[1];
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = sprite[0];
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        Destroy(lifeManager.GetComponent<LifeController>().hearts[carrotId]); //Destroy o objeto da cenoura armazenado no vetor (quando é criada)
        lifeManager.GetComponent<LifeController>().count = carrotId -= 2; //Diminiu o contador de quantidade de cenouras
    }
}
