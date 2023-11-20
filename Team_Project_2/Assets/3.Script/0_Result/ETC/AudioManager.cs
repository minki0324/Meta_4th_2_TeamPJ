using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private Optioin_Panel optionPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        optionPanel = FindObjectOfType<Optioin_Panel>();
    }

    public void SetVolume(float volume, string soundtype)
    {
        switch (soundtype)
        {
            case "Master":
                volume = optionPanel.masterVolume_Slider.value;
                break;


            case "Bgm":
                volume = optionPanel.bgmVolume_Slider.value;
                break;


            case "Sfx":
                volume = optionPanel.sfxVolume_Slider.value;
                break;
        }

        if (volume == 0f)
        {
            optionPanel.audioMixer.SetFloat(soundtype, -80);
        }
        else
        {
            optionPanel.audioMixer.SetFloat(soundtype, volume);
        }
    }

}
