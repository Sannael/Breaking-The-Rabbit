using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameObject[] hearts; //Lista com as imagens das cenouras


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (GameObject.Find("Player").GetComponent<PlayerScript>().health)
        {
            case 0:
                hearts[0].SetActive(false);
                hearts[1].SetActive(false);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 1:
                hearts[0].GetComponent<CarrotSpriteControl>().TrocaSprite(0);
                hearts[0].SetActive(true);
                hearts[1].SetActive(false);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 2:
                hearts[0].GetComponent<CarrotSpriteControl>().TrocaSprite(1);
                hearts[1].SetActive(false);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 3:
                hearts[1].GetComponent<CarrotSpriteControl>().TrocaSprite(0);
                hearts[1].SetActive(true);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 4:
                hearts[1].GetComponent<CarrotSpriteControl>().TrocaSprite(1);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 5:
                hearts[2].GetComponent<CarrotSpriteControl>().TrocaSprite(0);
                hearts[2].SetActive(true);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 6:
                hearts[2].GetComponent<CarrotSpriteControl>().TrocaSprite(1);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                break;
            case 7:
                hearts[3].GetComponent<CarrotSpriteControl>().TrocaSprite(0);
                hearts[3].SetActive(true);
                hearts[4].SetActive(false);
                break;
            case 8:
                hearts[3].GetComponent<CarrotSpriteControl>().TrocaSprite(1);
                hearts[4].SetActive(false);
                break;
            case 9:                
                hearts[4].GetComponent<CarrotSpriteControl>().TrocaSprite(0);
                hearts[4].SetActive(true);
                break;
            case 10:
                hearts[4].GetComponent<CarrotSpriteControl>().TrocaSprite(1);
                break;

        }
        
    }

}
