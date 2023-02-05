using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotSpriteControl : MonoBehaviour
{
    public Sprite[] sprite;
    public int spriteChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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


}
