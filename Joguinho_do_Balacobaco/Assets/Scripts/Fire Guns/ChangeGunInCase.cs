using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGunInCase : MonoBehaviour
{
    public void ChangeGun(GameObject newGun)
    {
        GameObject newg =Instantiate(newGun, transform.position, Quaternion.identity);
        newg.transform.parent = transform;
    }
}
