using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGMList
{
    MainBGM = 0
}

public enum SFXList
{
    MouseHover = 0,
    MouseClick,
    MouseClick2,
    StartGame,
    Arrow_Alway,
    Arrow_Shoot,
    Gate_Inter,
    Gate_Crash,
    Sword_Hit,
    Sword_Swing
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private Optioin_Panel optionPanel;

    [Header("오디오 믹서 (직접 참조)")]
    [SerializeField]
    private AudioMixer audioMixer;

    [Header("오디오 소스")]
    [SerializeField]
    public AudioSource audio_SFX;


    [Header("오디오 클립(직접 참조)")]
    public AudioClip[] clip_BGM;
    
    [SerializeField]
    public AudioClip[] clip_SFX;

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


    private void OnMouseEnter()
    {
        audio_SFX.PlayOneShot(clip_SFX[3]);
                        
    }

    public void SetVolume(float volume, string soundtype)
    {
        switch (soundtype)
        {
            case "Master":
                volume = Optioin_Panel.instance.masterVolume_Slider.value;
                break;


            case "Bgm":
                volume = Optioin_Panel.instance.bgmVolume_Slider.value;
                break;


            case "Sfx":
                volume = Optioin_Panel.instance.sfxVolume_Slider.value;
                break;
        }

        if (volume == -20f)
        {
            audioMixer.SetFloat(soundtype, -80f);
        }
        else
        {
            audioMixer.SetFloat(soundtype, volume);
        }
    }

    public void SFXPlay(int idx)   // SFX 플레이
    {
        audio_SFX.clip = clip_SFX[idx];
        audio_SFX.Play();
    }

    public void Button_HoverSound()
    {
        audio_SFX.clip = clip_SFX[0];
        
        audio_SFX.Play();
    }

}
