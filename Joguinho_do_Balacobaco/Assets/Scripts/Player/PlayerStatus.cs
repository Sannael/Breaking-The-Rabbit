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
    public int starFruitCount;
    public int starFruitMax;
    public bool confusion;
    public int extraLife;
    public float stunTime;
    public Dictionary<string, object> status = new Dictionary<string, object>();
    FieldInfo[] scriptVars = typeof(PlayerStatus).GetFields(BindingFlags.Public | BindingFlags.Instance);
    
    public void FillList()
    {
        foreach (FieldInfo variable in scriptVars)
        {
            object varValue = variable.GetValue(this);
            string name = variable.Name;
            status.Add(name, varValue);
        }
    }

}
