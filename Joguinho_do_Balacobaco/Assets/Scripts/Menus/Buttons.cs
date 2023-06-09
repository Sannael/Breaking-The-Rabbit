using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public AudioSource buttonSource;
    public void Onclick()
    {
        buttonSource.Play();
    }
}
