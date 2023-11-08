using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupationHUD : MonoBehaviour
{
    // 점령 HUD
    // 1. 점령이 끝나면 인게임 상단에 점령현황 ImageColor 변경
    // 2. 플레이어한테는 점령 중 Slider와 UI 표시되도록   


    [Header("색 변경")]
    [SerializeField] private Image[] Occu_Img_Color; // 점령 중인 팀 색

    private void Awake()
    {
        Occu_Img_Color = GetComponentsInChildren<Image>();

    }

    private void Update()
    {

    }

    public void ObjEnable(bool act)
    {
        Occu_Img_Color[1].transform.parent.gameObject.SetActive(act);

    }

    public void Change_HUD_Color(int Teamcolor)
    {
        // 나중에 컬러별로 수정

        Occu_Img_Color[0].color = ColorManager.instance.Teamcolor[Teamcolor];
        Occu_Img_Color[1].color = ColorManager.instance.Teamcolor[Teamcolor];
    }


}
