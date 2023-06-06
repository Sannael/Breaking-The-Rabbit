using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    public PolygonCollider2D[] polCollider; //1 ou mais colisores da arma
    public Animator meleeAnim; 
    public int damage;
    public int trueDamage;
    public GameObject gunCase;
    public GameObject thisMeleeChange;
    public Item item;
    [HideInInspector]
    public GameObject melee;

    public void Awake()
    {
        CoreInventory._instance.inventory.GetItem(item, 0, true, false, 2);
        melee = item.thisPrefabDrop.GetComponent<ChangeMelee>().melee;
        gunCase = gameObject.GetComponentInParent<MeleeController>().gunCase;
    }
    void Start()
    {
        this.GetComponent<DamageScript>().damage = damage;
        this.GetComponent<DamageScript>().trueDamage = trueDamage;
        meleeAnim.SetTrigger("Attack");
    }

    public void SaveMelee()
    {
        MeleeSaveStatus meleeSave = Resources.LoadAll("", typeof(MeleeSaveStatus)).Cast<MeleeSaveStatus>().First();
        FieldInfo[] scriptVars = typeof(MeleeScript).GetFields(BindingFlags.Public | BindingFlags.Instance);
        meleeSave.FillList();
        foreach(FieldInfo variable in scriptVars)
        {
            object varValue = variable.GetValue(this);
            string name = variable.Name;
            meleeSave.SaveList(name, varValue);
        }
    }
    public void TakeMelee(MeleeSaveStatus meleeSave)
    {
        meleeSave.FillList();
        foreach(var save in meleeSave.save)
        {
            TakeMeleeInfo(save.Key, save.Value);
        }
    }

    public void TakeMeleeInfo(string statusName, object value)
    {
        switch (statusName)
        {
            case "damage":
            damage = System.Convert.ToInt32(value);
            break;

            case "trueDamage":
            trueDamage = System.Convert.ToInt32(value);
            break;

            case "melee":
            melee = (GameObject)value; 
            break;
        }
    }


    void Update()
    {  
    }

    public void DisableCollider(int collider) //Desativar o colisor
    {
        polCollider[collider].enabled = false;
    }
    public void EnableCollider(int collider) //Ativar o colisor
    {
        polCollider[collider].enabled = true;
    }
    public void EndOfAtk()
    {
        gunCase.SetActive(true);
        gameObject.SetActive(false);
    } 
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("EnemyAtk"))
        {
            Destroy(other.gameObject);
        }    
    }
}
