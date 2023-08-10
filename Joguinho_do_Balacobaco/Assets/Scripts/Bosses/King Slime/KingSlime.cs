using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingSlime : MonoBehaviour
{
    public Animator anim;  
    public GameObject support;
    [Header("Sounds")]
    public AudioClip slimeKingSpawn;
    public AudioClip slimeLook;
    public void Sounds(AudioClip aud)
    {
        GameSounds.instance.CreateNewSound(aud);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(1);
    }
    public void SpawnSupport()
    {
        support.GetComponent<Animator>().SetTrigger("Start");
    }
    private void OnBecameVisible() 
    {
        GameObject.Find("GameController").GetComponent<GameController>().numberOfEnemies ++;    
    }
}
