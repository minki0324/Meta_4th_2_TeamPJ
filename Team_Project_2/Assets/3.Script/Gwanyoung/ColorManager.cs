using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // ColorManager
    // 깃발, HUD 색을 팀 색으로 변경하기 위함
    
    public static ColorManager instance = null;

    // 팀별로 컬러 지정 일단 싱글톤으로 해야할 것 같아서 싱글톤으로좀 하겠습니동

    public Color[] Teamcolor; // 팀 컬러
    public Material[] Flag_Color; // 깃발 색바꿀 Marterial

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


        Teamcolor = new Color[System.Enum.GetValues(typeof(Team)).Length];           
        Teamcolor[(int)Team.neutrality] = new Color(255, 255, 255); // 흰색
        Teamcolor[(int)Team.Team1] = new Color(255, 0, 0);  // 빨간색
        Teamcolor[(int)Team.Team2] = new Color(0, 170, 0);  // 초록색
        Teamcolor[(int)Team.Team3] = new Color(0, 0, 255);  // 파란색
        Teamcolor[(int)Team.Team4] = new Color(230, 0, 230); // 핑크색
    }
    

}
