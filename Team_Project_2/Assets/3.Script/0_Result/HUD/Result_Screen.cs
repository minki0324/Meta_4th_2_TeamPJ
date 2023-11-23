using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Screen : MonoBehaviour
{
    public enum InfoType
    {
        ColorSet, Mode, Time, Speed, TeamPoint, Gold, Hire, Kill, Death, Heal, Upgrade
    }
    public enum Team
    {
        Team1, Team2, Team3, Team4
    }

    public Team team;
    public InfoType type;
    [SerializeField] private Image[] images;
    [SerializeField] private Text[] Texts;

    private void OnEnable()
    {
        switch(type)
        {
            case InfoType.ColorSet:
                images[0].color = ColorManager.instance.Teamcolor[GameManager.instance.Color_Index];
                images[1].color = ColorManager.instance.Teamcolor[GameManager.instance.T1_Color];
                images[2].color = ColorManager.instance.Teamcolor[GameManager.instance.T2_Color];
                images[3].color = ColorManager.instance.Teamcolor[GameManager.instance.T3_Color];

                // ���� ���� ����
                float alphaValue = 0.4f; 

                images[0].color = new Color(ColorManager.instance.Teamcolor[GameManager.instance.Color_Index].r,
                    ColorManager.instance.Teamcolor[GameManager.instance.Color_Index].g,
                    ColorManager.instance.Teamcolor[GameManager.instance.Color_Index].b, alphaValue);
                images[1].color = new Color(ColorManager.instance.Teamcolor[GameManager.instance.T1_Color].r,
                    ColorManager.instance.Teamcolor[GameManager.instance.T1_Color].g,
                    ColorManager.instance.Teamcolor[GameManager.instance.T1_Color].b, alphaValue);
                images[2].color = new Color(ColorManager.instance.Teamcolor[GameManager.instance.T2_Color].r,
                     ColorManager.instance.Teamcolor[GameManager.instance.T2_Color].g,
                      ColorManager.instance.Teamcolor[GameManager.instance.T2_Color].b, alphaValue);
                images[3].color = new Color(ColorManager.instance.Teamcolor[GameManager.instance.T3_Color].r,
                    ColorManager.instance.Teamcolor[GameManager.instance.T3_Color].g,
                    ColorManager.instance.Teamcolor[GameManager.instance.T3_Color].b, alphaValue);
                break;
            case InfoType.Mode:
                if (GameManager.instance.GameMode == 0)
                {
                    Texts[0].text = "Original";
                }
                else
                {
                    Texts[0].text = "Time Attack";
                }
                break;
            case InfoType.Speed:
                if (GameManager.instance.GameSpeed == 0)
                {
                    Texts[0].text = "Normal Mode";
                }
                else
                {
                    Texts[0].text = "Speed Mode";
                }
                break;
            case InfoType.Time:
                Texts[0].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.currentTime / 60), ((int)GameManager.instance.currentTime) % 60); // ���� ���� �ð�
                break;
            case InfoType.TeamPoint:
                Texts[0].text = $"{(int)GameManager.instance.Teampoint}";
                Texts[1].text = $"{(int)GameManager.instance.leaders[0].Teampoint}";
                Texts[2].text = $"{(int)GameManager.instance.leaders[1].Teampoint}";
                Texts[3].text = $"{(int)GameManager.instance.leaders[2].Teampoint}";
                break;
            case InfoType.Gold:
                Texts[0].text = $"{(int)GameManager.instance.total_Gold}";
                Texts[1].text = $"{(int)GameManager.instance.leaders[0].total_Gold}";
                Texts[2].text = $"{(int)GameManager.instance.leaders[1].total_Gold}";
                Texts[3].text = $"{(int)GameManager.instance.leaders[2].total_Gold}";
                break;
            case InfoType.Hire:
                Texts[0].text = $"{(int)GameManager.instance.Hire}";
                Texts[1].text = $"{(int)GameManager.instance.leaders[0].Hire}";
                Texts[2].text = $"{(int)GameManager.instance.leaders[1].Hire}";
                Texts[3].text = $"{(int)GameManager.instance.leaders[2].Hire}";
                break;
            case InfoType.Kill:
                Texts[0].text = $"{(int)GameManager.instance.killCount}";
                Texts[1].text = $"{(int)GameManager.instance.leaders[0].killCount}";
                Texts[2].text = $"{(int)GameManager.instance.leaders[1].killCount}";
                Texts[3].text = $"{(int)GameManager.instance.leaders[2].killCount}";
                break;
            case InfoType.Death:
                Texts[0].text = $"{(int)GameManager.instance.DeathCount}";
                Texts[1].text = $"{(int)GameManager.instance.leaders[0].deathCount}";
                Texts[2].text = $"{(int)GameManager.instance.leaders[1].deathCount}";
                Texts[3].text = $"{(int)GameManager.instance.leaders[2].deathCount}";
                break;
            case InfoType.Heal:
                break;
            case InfoType.Upgrade:
                switch(team)
                {
                    case Team.Team1:
                        ApplyUpgradesToObjects(images, GameManager.instance.Upgrade_List);
                        break;
                    case Team.Team2:
                        ApplyUpgradesToObjects(images, GameManager.instance.leaders[0].Upgrade_List);
                        break;
                    case Team.Team3:
                        ApplyUpgradesToObjects(images, GameManager.instance.leaders[1].Upgrade_List);
                        break;
                    case Team.Team4:
                        ApplyUpgradesToObjects(images, GameManager.instance.leaders[2].Upgrade_List);
                        break;
                }
                break;
        }
    }

    private void ApplyUpgradesToObjects(Image[] images, List<int> upgradeList)
    {
        foreach (int index in upgradeList)
        {
            if (index >= 0 && index < images.Length)
            {
                GameObject objectToActivate = images[index].gameObject;
                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                }
            }
        }
    }
}
