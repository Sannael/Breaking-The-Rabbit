using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StopFalling());
    }

    public IEnumerator StopFalling()
    {
        Vector2 pos = transform.position; //Valor random, pra n bugar samerda
        pos[0] = 0;
        pos[1] = 0;
        yield return new WaitForSeconds(0.2f); 
        this.GetComponent<Rigidbody2D>().velocity = pos; //Parar a quada da moeda
        this.GetComponent<Rigidbody2D>().gravityScale = 0; //Tirar a gravidade da moeda
        
        float range = Random.Range(-0.05f, 0.05f); //Daqui pra baixo é só um esquema pra moeda se mexer um poquinho pra colidir uma com a outra
        Vector3 lastPos = transform.position;
        lastPos[0] = lastPos[0] + range;
        transform.position = Vector2.MoveTowards(transform.position, lastPos, 1);
    }
}