using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
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

        optionPanel = FindObjectOfType<Optioin_Panel>();

       
    }

    private void Start()
    {
        Init_AudioSource();
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

        if (volume == 0f)
        {
            audioMixer.SetFloat(soundtype, -80);
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
                //audio_SFX.PlayOneShot(clip_SFX[1]);
                break;

            case 1:
                audio_SFX.clip = clip_SFX[2];
                audio_SFX.Play();
                // audio_SFX.PlayOneShot(clip_SFX[2]);
                break;
        }
       
    }

}
