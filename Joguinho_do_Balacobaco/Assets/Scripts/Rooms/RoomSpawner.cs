using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;
    // 1 --> com porta TOP
    // 2 --> com porta DOWN
    // 3 --> com porta RIGHT
    // 4 --> com porta LEFT

    private RoomTemplates templates;
    private int rand; //variavel para determinar qual sala do grupo ser� spawnada
    private bool spawned = false; // vari�vel para determinar se uma sala j� foi spawnada

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>(); //basicamente especificar quais objetos podem cair nessa vari�vel, pela tag
        Invoke("RoomSpawn", 0.1f); // invocar a fun��o de spawnar salas com um intervalo de tempo determinado para garantir que as colis�es sejam detectadas corretamente
        Debug.Log("Fun��o de spawn invocada");
    }

    void RoomSpawn()
    {
        if(!spawned){
            if (openingDirection == 1)
            {
                // precisa spawnar outra sala que tenha TOP door
                rand = Random.Range(0, templates.topRooms.Length); // a sala escolhida varia entre a de posi��o 0 e m�xima posi��o do array "topRooms"
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation); //par�metros p/ instanciar
                Debug.Log("Nova sala TOP spawnada");
            }
            else if (openingDirection == 2)
            {
                // precisa spawnar outra sala que tenha DOWN door
                rand = Random.Range(0, templates.downRooms.Length); // a sala escolhida varia entre a de posi��o 0 e m�xima posi��o do array "downRooms"
                Instantiate(templates.downRooms[rand], transform.position, templates.downRooms[rand].transform.rotation);
                Debug.Log("Nova sala DOWN spawnada");
            }
            else if (openingDirection == 3)
            {
                // precisa spawnar outra sala que tenha RIGHT door
                rand = Random.Range(0, templates.rightRooms.Length); // a sala escolhida varia entre a de posi��o 0 e m�xima posi��o do array "rightRooms"
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                Debug.Log("Nova sala RIGHT spawnada");
            }
            else if (openingDirection == 4)
            {
                // precisa spawnar outra sala que tenha LEFT door
                rand = Random.Range(0, templates.leftRooms.Length); // a sala escolhida varia entre a de posi��o 0 e m�xima posi��o do array "leftRooms"
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                Debug.Log("Nova sala LEFT spawnada");
            }
            spawned = true; // ap�s uma sala ser instanciada, evita que outra seja spawnada por cima
        }
    }

    void OnTriggerEnter2D(Collider2D other) //ser� chamada toda vez que um "RoomSpawner" colidir com alguma coisa. O par�metro de detec��o � "qualquer outro colisor 2D"
    {
        if(other.CompareTag("RoomSpawner")) //se o colisor for de um objeto com a tag "RoomSpawner"
        {
            Destroy(gameObject); //evitando que ele instancie outra sala por cima
        }
    }

}
