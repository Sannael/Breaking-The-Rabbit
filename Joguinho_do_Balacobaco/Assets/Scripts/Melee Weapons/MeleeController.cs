using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private bool canAtk;
    public float atkSpeed;
    private float currentAtkSpeed;
    public GameObject weapon; //Arma corpo a corpo
    public GameObject gunCase;
    private GameController gameControllerScript;
    void Start()
    {
        currentAtkSpeed = 0f;
        gunCase = GameObject.Find("Player/GunCase");
        gameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAtkSpeed >0) //Se o cooldown n for 0 ir dimininuindo até zerar 
        {
            canAtk = false;
            currentAtkSpeed -= Time.deltaTime;
        }
        else
        {
            canAtk = true;
        }
        if(Input.GetMouseButtonDown(0) && canAtk == true && gameControllerScript.isPaused == false)
        {
            currentAtkSpeed = atkSpeed;
            weapon.SetActive(true);
            gunCase.SetActive(false);
        }
    }
}
