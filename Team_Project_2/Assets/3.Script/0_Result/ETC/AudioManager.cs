using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGMSound
{
    MainBGM = 0
}

public enum SFXSound
{
    MouseHove = 0,
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
    AudioMixer audioMixer;

    [Header("오디오 소스")]
    [SerializeField]
    public AudioSource audio_Master;

    [SerializeField]
    public AudioSource audio_BGM;

    [SerializeField]
    public AudioSource audio_SFX;


    [Header("오디오 클립(직접 참조)")]
    [SerializeField]
    private AudioClip[] clip_BGM;
    
    [SerializeField]
    private AudioClip[] clip_SFX;

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
        Init_AudioSource();
        optionPanel = FindObjectOfType<Optioin_Panel>();

       
    }


    private void OnMouseEnter()
    {
        audio_SFX.PlayOneShot(clip_SFX[3]);
                        
    }


    public void Init_AudioSource()
    {
        audio_Master = transform.GetChild(0).GetComponent<AudioSource>();
        audio_BGM = transform.GetChild(1).GetComponent<AudioSource>();
        audio_SFX = transform.GetChild(2).GetComponent<AudioSource>();
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

    
    public void BGMPlay(int idx)    // BGM 플레이
    {
        audio_BGM.clip = clip_BGM[idx];
        audio_BGM.Play();
    }
    public void SFXPlay(int idx)   // SFX 플레이
    {
        audio_SFX.clip = clip_SFX[idx];
        audio_SFX.Play();
    }
    public void SFXLoop(bool isLoop)
    {
        audio_SFX.loop = isLoop;
    }



    public void Button_HoverSound()
    {
        audio_SFX.clip = clip_SFX[0];
        
        audio_SFX.Play();
    }

    public void Button_ClickSound()
    {
        int clickSound = Random.Range(0, 2);

        switch(clickSound)
        {
            case 0:
                audio_SFX.clip = clip_SFX[1];
                audio_SFX.Play();
                break;

            case 1:
                audio_SFX.clip = clip_SFX[2];
                audio_SFX.Play();
                break;
        }
       
    }

}
