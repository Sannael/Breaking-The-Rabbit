using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSave", menuName = "Scriptable/SoundSave")]
public class SoundSave : ScriptableObject 
{
    public AudioMixer master;
    public float[] soundVolumes = new float[3];
    public float[] volumesBeforeMute = new float[3];
    public bool[] isMute = new bool[3];
    public void RecoverSounds()
    {
        master.SetFloat("MasterVolume", soundVolumes[0]);
        master.SetFloat("MusicVolume", soundVolumes[1]);
        master.SetFloat("SFXVolume", soundVolumes[2]);
    }
}
