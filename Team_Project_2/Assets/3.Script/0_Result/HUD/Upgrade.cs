using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    /*
        1. 유닛 맥스 카운트
        2. 사망 리스폰 타임 감소
        3. 인덱스 1번 병사 고용
        4. 인덱스 2번 병사 고용
        5. 리더 공격력 증가
        6. 리더 체력 증가
        7. 병사 공격력 증가
        8. 병사 체력 증가
        9. 이동속도 증가
        10. 골드 수급 증가
    */

    [SerializeField] private UpgradeData[] Data;
    [SerializeField] private Text[] Name_Texts;
    [SerializeField] private Image[] Icon_Images;
    [SerializeField] private Text[] Des_Texts;
    [SerializeField] private Text[] Cost_Texts;
    [SerializeField] private Button[] UpIndex_Buttons;


    [SerializeField] private Image UI;
    [SerializeField] private Text Damage;
    [SerializeField] private Text HP;
    [SerializeField] private GameObject Upgrade1_Ob;
    [SerializeField] private GameObject Upgrade2_Ob;
    [SerializeField] private GameObject Content;
    [SerializeField] private Button[] buttons;
    int DamageUpgradeCount = 0;
    int HPUpgradeCount = 0;

    private void OnEnable()
    {
        for(int i = 0; i < UpIndex_Buttons.Length; i++)
        {
            if (i == 2)
            {
                Name_Texts[i].text = GameManager.instance.unit1.unitName;
                Des_Texts[i].text = GameManager.instance.unit1.unitName + Data[i].Upgrade_Des;
            }
            else if (i == 3) 
            {
                Name_Texts[i].text = GameManager.instance.unit2.unitName;
                Des_Texts[i].text = GameManager.instance.unit2.unitName + Data[i].Upgrade_Des;
            }
            else
            {
                Name_Texts[i].text = Data[i].Upgrade_Name;
                Des_Texts[i].text = Data[i].Upgrade_Des;
            }
            Icon_Images[i].sprite = Data[i].Upgrade_Icon;
            Cost_Texts[i].text = $"Cost : {Data[i].Upgrade_Cost} gold";
        }
    }

    public void Upgrade_MaxCountUp()
    {
        if (GameManager.instance.Gold >= Data[0].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[0].Upgrade_Cost;
            GameManager.instance.Max_MinionCount += (int)Data[0].Value;
            UpIndex_Buttons[0].interactable = false;
            Debug.Log(GameManager.instance.Max_MinionCount);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }
    }

    public void Upgrade_RespawnTime()
    {
        if (GameManager.instance.Gold >= Data[1].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[1].Upgrade_Cost;
            GameManager.instance.respawnTime -= Data[1].Value;
            UpIndex_Buttons[1].interactable = false;
            Debug.Log(GameManager.instance.respawnTime);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Upgrade_Sol1()
    {
        if(GameManager.instance.Gold >= Data[2].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[2].Upgrade_Cost;
            GameManager.instance.isPossible_Upgrade_1 = true;
            Upgrade1_Ob.SetActive(true);
            UpIndex_Buttons[2].interactable = false;
            Debug.Log("1번째 업그레이드 완료");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Upgrade_Sol2()
    {
        if (GameManager.instance.Gold >= Data[3].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[3].Upgrade_Cost;
            GameManager.instance.isPossible_Upgrade_2 = true;
            Upgrade2_Ob.SetActive(true);
            UpIndex_Buttons[3].interactable = false;
            Debug.Log("2번째 업그레이드 완료");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Upgrade_LeaderATK()
    {
        if (GameManager.instance.Gold >= Data[4].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[4].Upgrade_Cost;
            GameManager.instance.Damage += Data[4].Value;
            UpIndex_Buttons[4].interactable = false;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Upgrade_LeaderHP()
    {
        if (GameManager.instance.Gold >= Data[5].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[5].Upgrade_Cost;
            GameManager.instance.Max_Hp += Data[5].Value;
            GameManager.instance.Current_HP += Data[5].Value;
            UpIndex_Buttons[5].interactable = false;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Upgrade_SolDAM()
    {
        if (GameManager.instance.Gold >= Data[6].Upgrade_Cost)
        {
            GameManager.instance.Gold -= Data[6].Upgrade_Cost;
            
            UpIndex_Buttons[5].interactable = false;
        }
    }

    public void Upgrade_SolHP()
    {

    }

    //Astar완성후 다시 작성
    public void Upgrade_Speed()
    {

    }
  
    public void Upgrade_Income()
    {

    }

}
