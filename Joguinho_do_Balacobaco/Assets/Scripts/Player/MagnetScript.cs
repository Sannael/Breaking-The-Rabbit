using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    public LayerMask layerObjectsMagnet = 9;
    public bool fixedForce = false;
    public float approximationCoefficient = 1.5f;
    public float magnetForce;
    public float mult;
    public float maxDistance;
    private float objectDistance;
    private float distanceSQRT;
    private float distanceSQRT_Force;

    private void Awake() {
        gameObject.layer = 9;
    }

    private void FixedUpdate() 
    {
        Transform p = GameObject.Find("Player").GetComponent<Transform>();
        transform.position = p.position;
        Collider2D[] objectsInDistance = Physics2D.OverlapCircleAll(transform.position, maxDistance, layerObjectsMagnet);
        foreach(Collider2D targetCollider in objectsInDistance)
        {
            CoinScript coin;
            StarFruit starFruit;
            AmmoDrop ammoDrop;
            bool objIsVisible = false;
            if(targetCollider.gameObject.TryGetComponent<CoinScript>(out coin)) //Tenta puxar script da moeda
            {
                objIsVisible = coin.isVisible;
            }
            else if(targetCollider.gameObject.TryGetComponent<StarFruit>(out starFruit)) //Ou da carambola
            {
                objIsVisible = starFruit.isVisible;
            }
            else if(targetCollider.gameObject.TryGetComponent<AmmoDrop>(out ammoDrop)) //Ou da munição, pra checar a visibilidade
            {
                objIsVisible = ammoDrop.isVisible;
            }

            if(targetCollider.gameObject.tag == "Coin" && objIsVisible == true) //Só puxa itens da tag coin que estiverem visiveis
            {
                targetCollider.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                Transform magnetTarget = targetCollider.transform;
                Rigidbody2D rbTemp = magnetTarget.GetComponent<Rigidbody2D>();
                if(rbTemp == true)
                {
                    Vector3 objectDirection = (transform.position - magnetTarget.position).normalized;
                    if(fixedForce == true)
                    {
                        distanceSQRT = (approximationCoefficient * approximationCoefficient);
                    }
                    else
                    {
                        objectDistance = Vector3.Distance(transform.position, magnetTarget.position);
                        distanceSQRT = Mathf.Pow(objectDistance, approximationCoefficient);
                    }
                    distanceSQRT_Force = (magnetForce / distanceSQRT)* mult;
                    
                    rbTemp.AddForce(objectDirection * distanceSQRT_Force);
                    Debug.DrawLine(transform.position, magnetTarget.position, Color.red);
                }
            }
        }
    }
}
