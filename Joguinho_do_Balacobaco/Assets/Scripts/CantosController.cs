using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantosController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameVisible() //Quando o objeto se tornar visivel em qualquer camera do jogo (A do scene conta)
    {
        GameObject.Find("GameController").GetComponent<GameController>().cantos = this.GetComponent<SpriteRenderer>().sprite.ToString();
        
    }

}
