using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public GameObject soundPnl;
    public Slider[] soundSlider;
    public SoundSave soundSave;
    public Image[] soundButtons = new Image[3];
    public Sprite[] soundSprite = new Sprite[4];
    
    public void OpenCloseSoundPnl(bool open)
    {
        soundPnl.SetActive(open);
        if(open)
        {
            OnSoundPnlEnable();
        }
    }

    void Update()
    {
        for(int i =0; i < soundSlider.Length; i ++)
        {
            if(soundSlider[i].value > soundSlider[i].minValue && soundSave.isMute[i] == true)
            {
                soundSave.isMute[i] = false;
            }
            ChangeButtonsImage(i);
        }
    }
    public void ChangeButtonsImage(int id)
    {
        if(soundSave.isMute[id] == true || soundSlider[id].value == -80)
        {
            soundButtons[id].GetComponent<Image>().sprite = soundSprite[0];
        }
        else if(soundSlider[id].value < -52 && soundSlider[id].value > -80)
        {
            soundButtons[id].GetComponent<Image>().sprite = soundSprite[1];
        }
        else if(soundSlider[id].value < -25 && soundSlider[id].value > -52)
        {
            soundButtons[id].GetComponent<Image>().sprite = soundSprite[2];
        }
        else if(soundSlider[id].value < 5 && soundSlider[id].value > -25)
        {
            soundButtons[id].GetComponent<Image>().sprite = soundSprite[3];
        }
    }
    public void MuteSoundTrack(int mute) //0 = all; 1 = music; 2 - SFX
    {
        if(mute == 0)
        {
            if(soundSave.isMute[mute] == false)
            {
                soundSave.volumesBeforeMute[mute] = soundSlider[mute].value;
                soundSlider[mute].value = soundSlider[mute].minValue;
                SetMasterVolume(soundSlider[mute].value);
            }   
            else
            {
                soundSlider[mute].value = soundSave.volumesBeforeMute[mute];
                SetMasterVolume(soundSlider[mute].value);
            }
            soundSave.isMute[mute] = !soundSave.isMute[mute];
            
        }
        else if(mute == 1)
        {
            if(soundSave.isMute[mute] == false)
            {
                soundSave.volumesBeforeMute[mute] = soundSlider[mute].value;
                soundSlider[mute].value = soundSlider[mute].minValue;
                SetMusicVolume(soundSlider[mute].value);
            }
            else
            {
                soundSlider[mute].value = soundSave.volumesBeforeMute[mute];
                SetMusicVolume(soundSlider[mute].value);
            }
            soundSave.isMute[mute] = !soundSave.isMute[mute];
            
        }
        else if(mute == 2)
        {
            if(soundSave.isMute[mute] == false)
            {
                soundSave.volumesBeforeMute[mute] = soundSlider[mute].value;
                soundSlider[mute].value = soundSlider[mute].minValue;
                SetSFXVolue(soundSlider[mute].value);
            }
            else
            {
                soundSlider[mute].value = soundSave.volumesBeforeMute[mute];
                SetSFXVolue(soundSlider[mute].value);
            }
            soundSave.isMute[mute] = !soundSave.isMute[mute];
            
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        soundSave.master.SetFloat("MasterVolume", sliderValue);
        soundSave.soundVolumes[0] = sliderValue;
    }
    public void SetMusicVolume(float sliderValue)
    {
        soundSave.master.SetFloat("MusicVolume", sliderValue);
        soundSave.soundVolumes[1] = sliderValue;
    }

    public void SetSFXVolue(float sliderValue)
    {
        soundSave.master.SetFloat("SFXVolume", sliderValue);
        soundSave.soundVolumes[2] = sliderValue;
    }

    private void OnSoundPnlEnable()
    {
        float[] volumes = new float[3];
        soundSave.master.GetFloat("MasterVolume", out volumes[0]);
        soundSave.master.GetFloat("MusicVolume", out volumes[1]);
        soundSave.master.GetFloat("SFXVolume", out volumes[2]);

        soundSlider[0].value = volumes[0];
        soundSlider[1].value = volumes[1];
        soundSlider[2].value = volumes[2];
    }
}
