using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleProceduralTerrainProject;

public class HUD : MonoBehaviour
{
    /*  
        관리 해야 하는 HUD 목록
        1. 시간 관리 (현재시간, 게임 종료 시간 출력)
        2. 병사 수 관리 (현재 병사 개수, 최대 병사 개수 출력)
        3. 체력 노출 (플레이어의 현재 체력, 최대 체력 노출)
        4. 골드 (현재 가지고 있는 골드 출력)
        5. 고용창 (현재 고용할 수 있는 병종 노출 (업그레이드 구현 후 고용할 수 없는 병종은 회색으로 출력되도록 수정)
    */

    public enum InfoType
    {
        Time, Soldier, Health, Gold, Employ, TeamPoint
    }

    public InfoType type;
    [SerializeField] private Text[] textarray;

    [SerializeField] private Ply_Controller ply;

    [SerializeField] private Slider HP_Slider;

    [SerializeField] private Image SliderImg;
    [SerializeField] private Image[] images;
    public Gradient HP_gradient;

    private void Update()
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }
        GameManager.instance.Teampoint = Score_Set(GameManager.instance.Ply_hasFlag, GameManager.instance.Teampoint);
        GameManager.instance.leaders[0].Teampoint = Score_Set(GameManager.instance.leaders[0].has_Flag, GameManager.instance.leaders[0].Teampoint);
        GameManager.instance.leaders[1].Teampoint = Score_Set(GameManager.instance.leaders[1].has_Flag, GameManager.instance.leaders[1].Teampoint);
        GameManager.instance.leaders[2].Teampoint = Score_Set(GameManager.instance.leaders[2].has_Flag, GameManager.instance.leaders[2].Teampoint);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        switch(type)
        {
            case InfoType.Time:
                textarray[0].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.currentTime / 60), ((int)GameManager.instance.currentTime) % 60); // 현재 시간
                textarray[1].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.EndTime / 60), ((int)GameManager.instance.EndTime) % 60); // 게임 종료 시간
                break;
            case InfoType.Soldier:
                textarray[0].text = GameManager.instance.Max_MinionCount.ToString();         // 최대 병사수
                textarray[1].text = GameManager.instance.Current_MinionCount.ToString();     // 현재 병사수
                break;
            case InfoType.Health:
                HP_Slider.value = GameManager.instance.Current_HP / GameManager.instance.Max_Hp;
                textarray[0].color = HP_gradient.Evaluate(GameManager.instance.Current_HP / GameManager.instance.Max_Hp);
                SliderImg.color = HP_gradient.Evaluate(GameManager.instance.Current_HP / GameManager.instance.Max_Hp); // 현재체력 비례 체력바 색변경
                textarray[0].text = $"{(int)GameManager.instance.Current_HP}<color=FFFFFF>/{(int)GameManager.instance.Max_Hp}</color>";   // 현재체력 / 총 체력
                textarray[1].text = string.Format("+{0:0.00}", GameManager.instance.Regeneration);
                break;
            case InfoType.Gold:
                textarray[0].text = $"골드: {(int)GameManager.instance.Gold}";
                break;
            case InfoType.Employ:
                textarray[0].text = $"♥80 /20\n고용 키: <color=#FF3E3E>1</color>\n비용: <color=#B7AF3D>15</color>"; // 검사
                textarray[1].text = $"♥{GameManager.instance.unit1.maxHP} /{GameManager.instance.unit1.damage}\n고용 키: <color=#FF3E3E>2</color>\n비용: <color=#B7AF3D>{GameManager.instance.unit1.cost}</color>"; // 기사
                textarray[2].text = $"♥{GameManager.instance.unit2.maxHP} /{GameManager.instance.unit2.damage}\n고용 키: <color=#FF3E3E>3</color>\n비용: <color=#B7AF3D>{GameManager.instance.unit2.cost}</color>"; // 궁수
                textarray[3].text = GameManager.instance.unit1.unitName.ToString();
                textarray[4].text = GameManager.instance.unit2.unitName.ToString();
   
                break;
            case InfoType.TeamPoint:
                images[0].color = ColorManager.instance.Teamcolor[GameManager.instance.Color_Index];
                images[1].color = ColorManager.instance.Teamcolor[GameManager.instance.T1_Color];
                images[2].color = ColorManager.instance.Teamcolor[GameManager.instance.T2_Color];
                images[3].color = ColorManager.instance.Teamcolor[GameManager.instance.T3_Color];

                textarray[0].text = $"{(int)GameManager.instance.Teampoint}";
                textarray[1].text = $"{(int)GameManager.instance.leaders[0].Teampoint}";
                textarray[2].text = $"{(int)GameManager.instance.leaders[1].Teampoint}";
                textarray[3].text = $"{(int)GameManager.instance.leaders[2].Teampoint}";
                break;
        }
    }

    private float Score_Set(int FlagCount, float score)
    {
        if(FlagCount == 0)
        {
            return score;
        }
        score += Time.deltaTime * (1 + (0.5f * (FlagCount - 1)));

        return score;
    }
}
