using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    public void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.75f);
            PlayerPrefs.SetFloat("SFXVolume", 0.75f);
        }
        else
        {
            audioMixer.SetFloat("music", Mathf.Log10(0.5f) * 20);
            audioMixer.SetFloat("sfx", Mathf.Log10(0.5f) * 20);
        }

    }

    public void SetMusicVolume()
    {
        float value = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void SetSFXVolume()
    {
        float value = SFXSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("sounSFXVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}
