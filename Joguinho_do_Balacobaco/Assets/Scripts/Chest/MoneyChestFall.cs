using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyChestFall : MonoBehaviour
{
    public Animator chestAnim;
    public ChestMoney chestMoney;
    [HideInInspector]
    public int dungeon;
    public GameObject coin;
    private float cameraY;
    private bool isOpen;
    [Header("Sounds")]
    public AudioClip openSound;
    public AudioClip fallChest;
    private bool chestSound;
    private void Start() 
    {
        chestSound = false;
        isOpen = false;
        cameraY = GameObject.FindGameObjectWithTag("MainCamera").transform.position[1];
        Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        pos[1] += 6.1f;
        pos[2] = 0;
        transform.position = pos;
    } 
    private void Update() 
    {
        if(transform.position[1] <= cameraY)
        {
            if(chestSound == false)
            {
                chestSound = true;
                GameSounds.instance.CreateNewSound(fallChest);
            }
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            if(isOpen == false)
            {
                isOpen = true;
                chestAnim.SetTrigger("Open");
                GameSounds.instance.CreateNewSound(openSound);
            }
        }
    }
    public void AmountCoins()
    {
        int moneyAmountDrop = chestMoney.DropCoins(dungeon);
        DropCoins(moneyAmountDrop);
    }
    public void DropCoins(int money)
    {
        for(int i =0; i <= money; i ++)
        {
            GameObject coinDrop = Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
