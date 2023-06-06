using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMeleeInCase : MonoBehaviour
{
    public void ChangeMelee(GameObject newMelee)
    {
        GameObject newm = Instantiate(newMelee, transform.position, Quaternion.identity);
        newm.transform.parent = transform;
    }
}
