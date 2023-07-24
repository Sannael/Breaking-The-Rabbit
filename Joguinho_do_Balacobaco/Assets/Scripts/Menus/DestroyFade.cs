using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFade : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
    public void AddOneExitTrigger()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().exitTriggers ++;
    }
}
