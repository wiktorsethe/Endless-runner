using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundEffectsSlider;
    [SerializeField] AudioSource musicAudio;
    [SerializeField] AudioSource[] soundEffectsAudio;
    private readonly Dictionary<string, AudioSource> _soundEffects = new Dictionary<string, AudioSource>();
    
    public static event Action<string> OnPlaySound;
    
    private void Awake()
    {
        OnPlaySound += PlaySound;
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            LoadMusic();
        }
        else
        {
            LoadMusic();
        }
        if (!PlayerPrefs.HasKey("soundEffectsVolume"))
        {
            PlayerPrefs.SetFloat("soundEffectsVolume", 0.5f);
            LoadSoundEffects();
        }
        else
        {
            LoadSoundEffects();
        }
        
        foreach (var audioSource in GetComponentsInChildren<AudioSource>())
        {
            _soundEffects[audioSource.gameObject.name] = audioSource;
        }
    }
    public void ChangeMusicVolume()
    {
        musicAudio.volume = musicSlider.value;
        
        TriggerSound("UIClick");
        SaveMusic();
    }
    public void ChangeSoundEffectsVolume()
    {
        foreach (AudioSource eachSource in soundEffectsAudio)
        {
            eachSource.volume = soundEffectsSlider.value;
        }
        
        TriggerSound("UIClick");
        SaveSoundEffects();
    }

    private void LoadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void LoadSoundEffects()
    {
        soundEffectsSlider.value = PlayerPrefs.GetFloat("soundEffectsVolume");
    }
    void SaveMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }
    void SaveSoundEffects()
    {
        PlayerPrefs.SetFloat("soundEffectsVolume", soundEffectsSlider.value);
    }
    
    private void PlaySound(string soundName)
    {
        if (_soundEffects.ContainsKey(soundName))
        {
            _soundEffects[soundName].Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found.");
        }
    }

    public static void TriggerSound(string soundName)
    {
        OnPlaySound?.Invoke(soundName);
    }
}