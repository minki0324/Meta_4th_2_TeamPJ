using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
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
    }



    public static AudioManager instance = null;

    private Optioin_Panel optionPanel;

    [Header("오디오 믹서 (직접 참조)")]
    [SerializeField]
    AudioMixer audioMixer;

    [Header("오디오 소스")]
    [SerializeField]
    private AudioSource audio_Master;

    [SerializeField]
    private AudioSource audio_BGM;

    [SerializeField]
    private AudioSource audio_SFX;


    [Header("오디오 클립(직접 참조)")]
    public AudioClip[] clip_BGM;
    
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

    private void Start()
    {
        Init_AudioSource();
        audio_BGM.clip = clip_BGM[0];
        audio_BGM.Play();
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


    public void Button_HoverSound()
    {
        audio_SFX.clip = clip_SFX[0];
        
        audio_SFX.Play();
    }

    public void Button_ClickSound()
    {
        //audio_SFX.clip = clip_SFX[1];
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
