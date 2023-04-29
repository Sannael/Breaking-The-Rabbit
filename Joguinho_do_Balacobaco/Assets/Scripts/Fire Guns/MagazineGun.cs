using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineGun : MonoBehaviour
{
    public float duration;
    public bool magRotate;
    public int maxRotation, rotationProgression;
    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        if(magRotate == true)
        {
            for(int i =0; i < maxRotation; i += rotationProgression)
            {
                transform.Rotate(0, 0, i);
                Debug.Log(i);
            }
        }
    }
}
