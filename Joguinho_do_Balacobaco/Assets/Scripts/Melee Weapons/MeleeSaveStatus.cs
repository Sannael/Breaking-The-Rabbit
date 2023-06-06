using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[CreateAssetMenu(fileName = "MeleeSaveStatus", menuName = "Scriptable/MeleeSaveStatus")]
public class MeleeSaveStatus : ScriptableObject
{
    public int damage;
    public int trueDamage;
    public GameObject melee;
    public Dictionary<string, object> save = new Dictionary<string, object>();
    FieldInfo[] scriptVars = typeof(MeleeSaveStatus).GetFields(BindingFlags.Public | BindingFlags.Instance);
    public void FillList()
    {
        foreach (FieldInfo variable in scriptVars)
        {
            object varValue = variable.GetValue(this);
            string name = variable.Name;
            if(save.ContainsKey(name))
            {
                save.Remove(name);
            }
            save.Add(name, varValue);
        }
    }

    public void SaveList(string statusName, object value)
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

    public void SetMelee()
    {
        GameObject oldMelee = Instantiate(melee);
        oldMelee.transform.SetParent(GameObject.FindGameObjectWithTag("MeleeCase").transform, false);
        oldMelee.GetComponentInChildren<MeleeController>().weapon.GetComponent<MeleeScript>().TakeMelee(this);
    }

    public void SetFirstMelee()
    {
        GameObject oldMelee = Instantiate(melee);
        oldMelee.transform.SetParent(GameObject.FindGameObjectWithTag("MeleeCase").transform, false);
    }
}
