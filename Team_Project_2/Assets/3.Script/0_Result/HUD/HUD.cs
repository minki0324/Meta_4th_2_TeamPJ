using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleProceduralTerrainProject;

public class HUD : MonoBehaviour
{
    /*  
        ���� �ؾ� �ϴ� HUD ���
        1. �ð� ���� (����ð�, ���� ���� �ð� ���)
        2. ���� �� ���� (���� ���� ����, �ִ� ���� ���� ���)
        3. ü�� ���� (�÷��̾��� ���� ü��, �ִ� ü�� ����)
        4. ��� (���� ������ �ִ� ��� ���)
        5. ���â (���� ����� �� �ִ� ���� ���� (���׷��̵� ���� �� ����� �� ���� ������ ȸ������ ��µǵ��� ����)
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
                textarray[0].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.currentTime / 60), ((int)GameManager.instance.currentTime) % 60); // ���� �ð�
                textarray[1].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.EndTime / 60), ((int)GameManager.instance.EndTime) % 60); // ���� ���� �ð�
                break;
            case InfoType.Soldier:
                textarray[0].text = GameManager.instance.Max_MinionCount.ToString();         // �ִ� �����
                textarray[1].text = GameManager.instance.Current_MinionCount.ToString();     // ���� �����
                break;
            case InfoType.Health:
                HP_Slider.value = GameManager.instance.Current_HP / GameManager.instance.Max_Hp;
                textarray[0].color = HP_gradient.Evaluate(GameManager.instance.Current_HP / GameManager.instance.Max_Hp);
                SliderImg.color = HP_gradient.Evaluate(GameManager.instance.Current_HP / GameManager.instance.Max_Hp); // ����ü�� ��� ü�¹� ������
                textarray[0].text = $"{(int)GameManager.instance.Current_HP}<color=FFFFFF>/{(int)GameManager.instance.Max_Hp}</color>";   // ����ü�� / �� ü��
                textarray[1].text = string.Format("+{0:0.00}", GameManager.instance.Regeneration);
                break;
            case InfoType.Gold:
                textarray[0].text = $"���: {(int)GameManager.instance.Gold}";
                break;
            case InfoType.Employ:
                textarray[0].text = $"��80 /20\n��� Ű: <color=#FF3E3E>1</color>\n���: <color=#B7AF3D>15</color>"; // �˻�
                textarray[1].text = $"��{GameManager.instance.unit1.maxHP} /{GameManager.instance.unit1.damage}\n��� Ű: <color=#FF3E3E>2</color>\n���: <color=#B7AF3D>{GameManager.instance.unit1.cost}</color>"; // ���
                textarray[2].text = $"��{GameManager.instance.unit2.maxHP} /{GameManager.instance.unit2.damage}\n��� Ű: <color=#FF3E3E>3</color>\n���: <color=#B7AF3D>{GameManager.instance.unit2.cost}</color>"; // �ü�
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
