using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleScript : MonoBehaviour
{
    public float duration; //Duração da capsula na cena
    void Start()
    {
        Destroy(gameObject, duration); //Destroy a capsula depois que o tempo passar
    }

    void Update()
    {  
    }
}
