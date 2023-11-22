using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Optioin_Panel : MonoBehaviour
{
    public static Optioin_Panel instance = null;

    
    #region �ɼ� �г�
    [Header("Option Panel")]

    [SerializeField]
    public Slider masterVolume_Slider;

    [SerializeField]
    public Slider bgmVolume_Slider;

    [SerializeField]
    public Slider sfxVolume_Slider;

    [SerializeField]
    private Slider mouseSensitive_Slider;

    [SerializeField]
    private Slider Brightness_Slider;

    [SerializeField]
    private Button windowScreen_Btn;

    [SerializeField]
    private Button fullScreen_Btn;


    [SerializeField]
    private Light light_;

    [SerializeField]
    private Image Background_img;

    public AudioMixer audioMixer;


    private float MouseY;
    private float MouseX;

    [SerializeField]
    CameraControl cameraControl;

    #endregion



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


    }


    public void OptionPanel_On()
    {
        this.gameObject.SetActive(true);



        GameObject Selection_img = transform.GetChild(0).gameObject;
        Background_img = transform.GetChild(1).GetComponent<Image>();
        cameraControl = FindObjectOfType<CameraControl>();

        #region ������Ʈ ����
        masterVolume_Slider = Selection_img.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        bgmVolume_Slider = Selection_img.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        sfxVolume_Slider = Selection_img.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        mouseSensitive_Slider = Selection_img.transform.GetChild(3).GetChild(0).GetComponent<Slider>();
        Brightness_Slider = Selection_img.transform.GetChild(4).GetChild(0).GetComponent<Slider>();

        windowScreen_Btn = Selection_img.transform.GetChild(5).GetChild(0).GetComponent<Button>();
        fullScreen_Btn = Selection_img.transform.GetChild(5).GetChild(1).GetComponent<Button>();
        #endregion


        //���� ���� �����̴�
        masterVolume_Slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(-10f, "Master"); });
        bgmVolume_Slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(-10f, "Bgm"); });
        sfxVolume_Slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(-10f, "Sfx"); });


        //��� ���� �����̴�
        //Brightness_Slider.onValueChanged.AddListener(SetBrightness);
        Brightness_Slider.onValueChanged.AddListener(SetBrightness_img);


        //���콺 �ӵ� �����̴� -> �ΰ��ӿ��� ī�޶� �ӵ� ����
        mouseSensitive_Slider.onValueChanged.AddListener(SetMouseSensitive);



        //â ũ�� ���� ��ư
        windowScreen_Btn.onClick.AddListener(delegate { SetScreenSize("Window"); });
        fullScreen_Btn.onClick.AddListener(delegate { SetScreenSize("Full"); });


    }


    //public void SetVolume(float volume, string soundtype)
    //{
    //    switch (soundtype)
    //    {
    //        case "Master":
    //            volume = masterVolume_Slider.value;
    //            break;


    //        case "Bgm":
    //            volume = bgmVolume_Slider.value;
    //            break;


    //        case "Sfx":
    //            volume = sfxVolume_Slider.value;
    //            break;
    //    }

    //    if (volume == 0f)
    //    {
    //        audioMixer.SetFloat(soundtype, -80);
    //    }
    //    else
    //    {
    //        audioMixer.SetFloat(soundtype, volume);
    //    }
    //}

    public void SetBrightness_img(float value)
    {
        value = Mathf.Abs(Brightness_Slider.value - 255);

        Background_img.color = new Color(Background_img.color.r, Background_img.color.g, Background_img.color.b, value / 255);
    }


    public void SetBrightness(float value)
    {
        light_ = FindObjectOfType<Light>();
        value = Brightness_Slider.value;

        if (value > 10)
        {
            value = 10;
        }


        light_.intensity = value;
    }


    private void SetMouseSensitive(float mouseSensitivity)
    {

        if (mouseSensitivity > 10)
        {
            mouseSensitivity = 10;
        }
        if (mouseSensitivity < 1)
        {
            mouseSensitivity = 1;
        }

        cameraControl.RotSen = mouseSensitivity;

    }


    public void SetScreenSize(string screen)
    {
        switch (screen)
        {
            case "Window":
                Debug.Log("Window Screen");
                Screen.SetResolution(1024, 768, false);
                break;

            case "Full":
                Debug.Log("Full Screen");

                Screen.SetResolution(1920, 1080, true);
                break;
        }
    }



}
