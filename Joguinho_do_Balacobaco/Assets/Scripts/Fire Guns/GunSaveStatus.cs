using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[CreateAssetMenu(fileName = "GunSaveStatus", menuName ="Scriptable/GunSaveStatus")]
public class GunSaveStatus : ScriptableObject
{
    public enum GunType{AssaultRifle, Magnum, Pistol, Revolver, Shotgun, SMG};
    public GunType gunType;
    public float gunRate;
    public int damage;
    public int trueDamage;
    public int ammo;
    public int totalAmmo;
    public int playerAmmo;
    public int bulletSpread;
    public int bulletSpreadMin;
    public int bulletSpreadMax;
    public GameObject gun;
    public Dictionary<string, object> save = new Dictionary<string, object>();
    FieldInfo[] scriptVars = typeof(GunSaveStatus).GetFields(BindingFlags.Public | BindingFlags.Instance);
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
            case "gunType":
            gunType = (GunType)value;
            break;

            case "gunRate":
            gunRate = System.Convert.ToSingle(value);
            break;

            case "damage":
            damage = System.Convert.ToInt32(value);
            break;

            case "trueDamage":
            trueDamage = System.Convert.ToInt32(value);
            break;

            case "ammo":
            ammo = System.Convert.ToInt32(value);
            break;

            case "totalAmmo":
            totalAmmo = System.Convert.ToInt32(value);
            break;

            case "playerAmmo":
            playerAmmo = System.Convert.ToInt32(value);
            break;

            case "bulletSpread":
            bulletSpread = System.Convert.ToInt32(value);
            break;

            case "bulletSpreadMin":
            bulletSpreadMin = System.Convert.ToInt32(value);
            break;

            case "bulletSpreadMax":
            bulletSpreadMax = System.Convert.ToInt32(value);
            break;

            case "gun":
            gun = (GameObject)value; 
            break;
        }
    }

    public void SetGun()
    {
        GameObject oldGun = Instantiate(gun);
        oldGun.transform.SetParent(GameObject.FindGameObjectWithTag("GunCase").transform, false);
        oldGun.GetComponentInChildren<GunStatus>().TakeGun(this);       
    }

    public void SetFirstGun()
    {
        GameObject oldGun = Instantiate(gun);
        oldGun.transform.SetParent(GameObject.FindGameObjectWithTag("GunCase").transform, false);
    }
}
