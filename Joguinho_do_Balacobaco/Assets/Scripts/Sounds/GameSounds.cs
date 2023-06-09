using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    public static GameSounds instance = null;
    public AudioSource sfxSource;

    [HideInInspector]
    public bool timeSc; 
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this; 
        }    
        else if(instance != this)
        {
            //Destroy(gameObject);
        }
    }

    public void Update()
    {
        if(timeSc == true)
        {
            if(Time.timeScale == 0)
            {
                sfxSource.Pause();
            }
            else if(sfxSource.isPlaying == false)
            {
                sfxSource.UnPause();
            }

        }
    }

    public GameObject CreateNewSoundLoop(AudioClip clip)
    {
        GameObject s = Instantiate(gameObject);
        s.GetComponent<GameSounds>().sfxSource.loop = true;
        s.GetComponent<GameSounds>().PlaySingle(clip);
        s.GetComponent<GameSounds>().timeSc = true;
        return s;
    }

    public void CreateNewSound(AudioClip clip)
    {
        GameObject s = Instantiate(gameObject);
        s.GetComponent<GameSounds>().PlaySingle(clip);
        s.GetComponent<GameSounds>().timeSc = true;
        Destroy(s, clip.length);
    }

    public void CreateNewSoundNoScale(AudioClip clip)
    {
        GameObject s = Instantiate(gameObject);
        s.GetComponent<GameSounds>().PlaySingle(clip);
        s.GetComponent<GameSounds>().timeSc = false;
        Destroy(s, clip.length);
    }


    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

}
