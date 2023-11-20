using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Optioin_Panel : MonoBehaviour
{
    public static Optioin_Panel instance = null;

   

    #region 옵션 패널
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

        #region 오브젝트 연결
        masterVolume_Slider = Selection_img.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        bgmVolume_Slider = Selection_img.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        sfxVolume_Slider = Selection_img.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        mouseSensitive_Slider = Selection_img.transform.GetChild(3).GetChild(0).GetComponent<Slider>();
        Brightness_Slider = Selection_img.transform.GetChild(4).GetChild(0).GetComponent<Slider>();

        windowScreen_Btn = Selection_img.transform.GetChild(5).GetChild(0).GetComponent<Button>();
        fullScreen_Btn = Selection_img.transform.GetChild(5).GetChild(1).GetComponent<Button>();
        #endregion


        //사운드 조절 슬라이더
        masterVolume_Slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(5, "Master"); });
        bgmVolume_Slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(5, "Bgm"); });
        sfxVolume_Slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(5, "Sfx"); });


        //밝기 조절 슬라이더
        //Brightness_Slider.onValueChanged.AddListener(SetBrightness);
        Brightness_Slider.onValueChanged.AddListener(SetBrightness_img);


        //마우스 속도 슬라이더 -> 인게임에서 카메라 속도 조절
        mouseSensitive_Slider.onValueChanged.AddListener(SetMouseSensitive);



        //창 크기 조절 버튼
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
        //나중에 CameraControl 값 가져오기
        if (mouseSensitivity > 100)
        {
            mouseSensitivity = 100;
        }

        MouseX += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        MouseY -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        MouseY = Mathf.Clamp(MouseY, -90f, 90f); //Clamp를 통해 최소값 최대값을 넘지 않도록함

        transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f);// 각 축을 한꺼번에 계산
    }


    public void SetScreenSize(string screen)
    {
        switch (screen)
        {
            case "Window":
                Debug.Log("Window Screen");
                Screen.SetResolution(1024, 768, true);
                break;

            case "Full":
                Debug.Log("Full Screen");

                Screen.SetResolution(1920, 1080, true);
                break;
        }
    }



}
