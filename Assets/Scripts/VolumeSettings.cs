using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource[] soundEffectsAudio;
    private float _musicVolume;
    private float _soundEffectsVolume;
    private void Start()
    {
        _musicVolume = PlayerPrefs.GetFloat("musicVolume");
        _soundEffectsVolume = PlayerPrefs.GetFloat("soundEffectsVolume");
        if(musicAudio)
        {
            musicAudio.volume = _musicVolume;
        }
        foreach (AudioSource eachSource in soundEffectsAudio)
        {
            eachSource.volume = _soundEffectsVolume;
        }
    }
}