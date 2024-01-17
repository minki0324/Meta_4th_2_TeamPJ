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
    Sword_Swing1,
    Sword_Swing2 = 10,
    Sword_Swing3,
    Wind_Storm,
    Flag_Sound,
    Human_Attack1,
    Human_Attack2,
    Human_Die1,
    Human_Die2,
    Human_Die3
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private Optioin_Panel optionPanel;

    [Header("오디오 믹서 (직접 참조)")]
    [SerializeField]
    private AudioMixer audioMixer;

    [Header("오디오 소스")]
    public AudioSource[] audio_SFX;


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

        audio_SFX = GetComponents<AudioSource>();
    }


    private void OnMouseEnter()
    {
        audio_SFX[0].PlayOneShot(clip_SFX[3]);
                        
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
        for (int i = 0; i < audio_SFX.Length; i++)
        {
            if (!audio_SFX[i].isPlaying)
            {
                audio_SFX[i].clip = clip_SFX[idx];
                audio_SFX[i].Play();
                break;
            }
        }
    }

    public void Button_HoverSound()
    {
        SFXPlay((int)SFXList.MouseHover);
    }

}
