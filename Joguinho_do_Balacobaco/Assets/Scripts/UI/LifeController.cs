using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public GameObject[] hearts; //Lista com as imagens das cenouras
    public GameObject img; //Arte da cenourinha
    public int playerCurrentHealth; //Vida atual do player
    public int playerHealth; //Vida anterior (Pré mudança)
    public int count; //Contagem de quantas cenouras tem ingame
    public int distanceBetweenCarrots;
    
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerScript>().health;
        playerCurrentHealth = GameObject.Find("Player").GetComponent<PlayerScript>().health;
        CreateHearths(); //Função que cria as cenouras
    }

    // Update is called once per frame
    void Update()
    {
        playerCurrentHealth = GameObject.Find("Player").GetComponent<PlayerScript>().health;
        if(playerCurrentHealth > playerHealth) //Se o player aumentar a vida maxima dele
        {
            CreateHearths();
            playerHealth = playerCurrentHealth; //Reseta o valor da vida
        }
        else if(playerCurrentHealth < playerHealth) //Se perder vida; não deixei pra atualizar sem nenhuma condição pra ter uma delay minimo e outra função funfar namoral
        {
            playerHealth = playerCurrentHealth;
        }
    }

    public void CreateHearths()
    {
        for(int i = count +1; i <= (playerCurrentHealth/2); i++) //Continha pra saber quantos conraçoes vão ser criados; caso a vida atual for par(cenoura full)
        {
            RectTransform rTransform = hearts[i -2].GetComponent<RectTransform>(); //Pega o Rect Transform
            Vector3 pos = rTransform.localPosition; //Armazena a posição da ultima cenoura em tela (direita pra esquerda)
            pos[0] += distanceBetweenCarrots; //Seta o X da nva cenoura pra ficar 40 pro lado
            hearts[i-1] = Instantiate(img, rTransform.localPosition, Quaternion.identity, this.gameObject.transform); //Istancia e armazena a cenourinha
            hearts[i-1].GetComponent<RectTransform>().localPosition = pos; //Muda a posição da cenoura; se n mudar manual aqui da ruim, ele vai pra 3k e pouco em X
            count = i; //Aumenta o contador de cenourinha
            hearts[i-1].GetComponent<CarrotSpriteControl>().carrotId = count; //armazena o Id da cenoura, basicamente a ordem dela
        }
        if(playerCurrentHealth % 2 >0) //Aqui faz a mesma coisa soq que se for impar resumidamente; se resto de divisão por 2 for >0 é um numero impar
        {
            RectTransform rTransform = hearts[count -1].GetComponent<RectTransform>();
            Vector3 pos = rTransform.localPosition;
            pos[0] += distanceBetweenCarrots;
            hearts[count] = Instantiate(img, rTransform.localPosition, Quaternion.identity, this.gameObject.transform);
            hearts[count].GetComponent<RectTransform>().localPosition = pos;
            count ++;
            hearts[count-1].GetComponent<CarrotSpriteControl>().carrotId = count;
        }
    }
}
