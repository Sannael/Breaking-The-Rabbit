using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference meleeAtk;
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
        weapon.GetComponent<MeleeScript>().Awake();
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
        if(meleeAtk.action.IsPressed() && canAtk == true && gameControllerScript.isPaused == false)
        {
            currentAtkSpeed = atkSpeed;
            weapon.SetActive(true);
            gunCase.SetActive(false);
            gunCase.GetComponentInChildren<GunStatus>().reloading = false; //Evita bugar o reloading (tlvz dps pense algo melhor kk)
        }
    }
}
