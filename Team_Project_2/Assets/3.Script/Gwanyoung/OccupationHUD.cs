using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupationHUD : MonoBehaviour
{
    // 점령 HUD
    // 1. 점령이 끝나면 인게임 상단에 점령현황 ImageColor 변경
    // 2. 플레이어한테는 점령 중 Slider와 UI 표시되도록   

    [SerializeField] private Image[] Occu_Img_Color; // 점령 중인 팀 색
    public Flag[] FlagArray;  // 플래그 컴포넌트 배열
    public Slider OccuSlider;
    private Color colorTemp;

    private void Awake()
    {
        FlagArray = FindObjectsOfType<Flag>();
        
        Occu_Img_Color = GetComponentsInChildren<Image>();
        OccuSlider = GetComponentInChildren<Slider>();
        for (int i = 0; i < 5; i++)
        {
            Occu_Img_Color[(i * 2) + 1].transform.parent.gameObject.SetActive(false);
        }

    }

    private void Update()
    {

    }

    public void ObjEnable(bool act)
    {
        Occu_Img_Color[1].transform.parent.gameObject.SetActive(act);

    }

    public void Change_Color(int TeamNum, int FlagNum)
    {        
        colorTemp = ColorManager.instance.Teamcolor[TeamNum];
        colorTemp.a = 0.4f;

        Occu_Img_Color[FlagNum * 2].color = colorTemp;
        Occu_Img_Color[FlagNum * 2 + 1].color = colorTemp;
        FlagArray[FlagNum].Change_Flag_Color(TeamNum);

    }


}
