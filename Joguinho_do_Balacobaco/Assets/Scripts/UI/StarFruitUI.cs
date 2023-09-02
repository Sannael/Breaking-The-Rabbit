using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarFruitUI : MonoBehaviour
{
    public Image[] starFruits;
    public bool[] haveStarFruit;
    public int starFruitCount;
    public PlayerScript ps;


    private void Start() 
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }
    private void Update() 
    {
        starFruitCount = ps.starFruitCount;
        ReCountStarFruit();
        ChangeImage();
    }

    public void ReCountStarFruit()
    {
        for(int i =1; i <= haveStarFruit.Length; i ++)
        {
            if(ps.starFruitMax >= i)
            {
                haveStarFruit[i-1] = true;
            }
            else
            {
                haveStarFruit[i-1] = false;
            }
        }
    }

    public void ChangeImage()
    {
        for(int i = 0; i < starFruits.Length; i ++)
        {
            if(haveStarFruit[i] == false) //Se o player n tiver a quantidade de carambola
            {   
                starFruits[i].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            }
            else if(starFruitCount >= (i+1) && haveStarFruit[i] == true)
            {   
                starFruits[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            else if(starFruitCount < (i+1) && haveStarFruit[i] == true)
            {   
                starFruits[i].GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            }
        }
    }
}
