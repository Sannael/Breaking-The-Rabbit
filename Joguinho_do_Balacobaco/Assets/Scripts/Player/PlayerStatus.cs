using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerStatus", menuName ="Scriptable/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    public float speed;
    public float rollCdr;
    public int health;
    public int maxHealth;
    public bool isAlive;
    public bool canTakeDamage;
    public int armor;
    public int coinCount;
    public int revolverAmmo;
    public int shotgunAmmo;
    public int pistolAmmo;
    public int assaultRifleAmmo;
    public int smgAmmo;
    public int magnumAmmo;
    public int starFruitMax;
    public bool confusion;
    public int extraLife;
    public float stunTime;
    public int shopDiscount;
    public Dictionary<string, object> status = new Dictionary<string, object>();
    FieldInfo[] scriptVars = typeof(PlayerStatus).GetFields(BindingFlags.Public | BindingFlags.Instance);
    
    public void FillList()
    {
        foreach (FieldInfo variable in scriptVars)
        {
            object varValue = variable.GetValue(this);
            string name = variable.Name;
            if(status.ContainsKey(name))
            {
                status.Remove(name);
            }
            status.Add(name, varValue);
        }
    }
    public void SaveList(string statusName, object newValue)
    {
        switch (statusName)
        {
            case "speed":
            speed = System.Convert.ToSingle(newValue);
            break;
            
            case "rollCdr":
            rollCdr = System.Convert.ToSingle(newValue);
            break;

            case "health":
            health = System.Convert.ToInt32(newValue);
            break;

            case "maxHealth":
            maxHealth = System.Convert.ToInt32(newValue);
            break;

            case "isAlive":
            isAlive = System.Convert.ToBoolean(newValue);
            break;

            case "canTakeDamage":
            canTakeDamage = System.Convert.ToBoolean(newValue);
            break;

            case "armor":
            armor = System.Convert.ToInt32(newValue);
            break;

            case "coinCount":
            coinCount = System.Convert.ToInt32(newValue);
            break;

            case "revolverAmmo":
            revolverAmmo = System.Convert.ToInt32(newValue);
            break;

            case "shotgunAmmo":
            shotgunAmmo = System.Convert.ToInt32(newValue);
            break;

            case "pistolAmmo":
            pistolAmmo = System.Convert.ToInt32(newValue);
            break;

            case "assaultRifleAmmo":
            assaultRifleAmmo = System.Convert.ToInt32(newValue);
            break;

            case "smgAmmo":
            smgAmmo = System.Convert.ToInt32(newValue);
            break;

            case "magnumAmmo":
            magnumAmmo = System.Convert.ToInt32(newValue);
            break;

            case "starFruitMax":
            starFruitMax = System.Convert.ToInt32(newValue);
            break;

            case "confusion":
            confusion = System.Convert.ToBoolean(newValue);
            break;

            case "extraLife":
            extraLife = System.Convert.ToInt32(newValue);
            break;

            case "shopDiscount":
            shopDiscount = System.Convert.ToInt32(newValue);
            break;
        }
    }

}
