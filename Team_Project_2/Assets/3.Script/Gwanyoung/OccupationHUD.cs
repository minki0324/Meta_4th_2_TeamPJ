using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupationHUD : MonoBehaviour
{
    // 점령 HUD
    // 1. 점령이 끝나면 인게임 상단에 점령현황 ImageColor 변경
    // 2. 플레이어한테는 점령 중 Slider와 UI 표시되도록   

    [SerializeField] GameObject[] Occu_image = null;
    [SerializeField] private Image[] Occu_Img_Color; // 점령 중인 팀 색
    public Flag[] FlagArray;  // 플래그 컴포넌트 배열
    public Slider[] OccuSlider;
    private Color ColorTemp;

    private void Start()
    {
        FlagArray = FindObjectsOfType<Flag>();
        Occu_image = new GameObject[7];
        Occu_Img_Color = GetComponentsInChildren<Image>();
        OccuSlider = GetComponentsInChildren<Slider>();
       
        for (int i = 0; i < FlagArray.Length; i++)
        {
            Occu_image[i] = Occu_Img_Color[i * 4].transform.parent.gameObject;
            Occu_image[i].transform.localPosition = new Vector3((-50 * FlagArray.Length * 0.5f) + (50 * i), 0, 0);
        }

        for(int i = FlagArray.Length; i < 7; i++)
        {
            Debug.Log(i);
            Occu_image[i] = Occu_Img_Color[i * 4].transform.parent.gameObject;
            Occu_image[i].SetActive(false);
        }



        for (int i = 0; i < Occu_Img_Color.Length * 0.5f; i++) 
        {
            Occu_Img_Color[(i * 2) + 1].transform.parent.gameObject.SetActive(false);
           
        }
    }    

    public void Ply_Slider(int TeamColor, int FlagNum, float Current, float Total)
    {
        ColorTemp = ColorManager.instance.Teamcolor[TeamColor];
        ColorTemp.a = 0.431f;

        Occu_Img_Color[FlagNum * 4 + 2].color = ColorTemp; 
        OccuSlider[FlagNum].value = Current / Total;   // 슬라이더 현재 게이지
    }
 
    public void Ply_OccuHUD(int FlagNum, bool Act)
    {
        // 점령중인 유닛이 Player 일 때 SetActive의 불값을 주기 위한 메서드
        Occu_Img_Color[FlagNum * 4 + 1].transform.parent.gameObject.SetActive(Act);  // 점령 중앙쪽 HUD
        Occu_Img_Color[FlagNum * 4 + 3].transform.parent.gameObject.SetActive(Act);  // 점령 슬라이더
    }

    public void Change_Color(int TeamColor, int FlagNum)
    {        
        ColorTemp = ColorManager.instance.Teamcolor[TeamColor];
        ColorTemp.a = 0.431f;

        Occu_Img_Color[FlagNum * 4].color = ColorTemp; // HUD 상단               

        Occu_Img_Color[FlagNum * 4 + 1].color = ColorTemp; // 플레이어에게 뜰 HUD   

        FlagArray[FlagNum].Change_Flag_Color(TeamColor); // 깃발 색 변경

    }


}
