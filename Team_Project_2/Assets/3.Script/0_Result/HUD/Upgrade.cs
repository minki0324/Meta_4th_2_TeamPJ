using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    /*
        1. ���� �ƽ� ī��Ʈ
        2. ��� ������ Ÿ�� ����
        3. �ε��� 1�� ���� ���
        4. �ε��� 2�� ���� ���
        5. ���� ���ݷ� ����
        6. ���� ü�� ����
        7. ���� ���ݷ� ����
        8. ���� ü�� ����
        9. �̵��ӵ� ����
        10. ��� ���� ����
    */

    [SerializeField] private UpgradeData[] Data;
    [SerializeField] private Text[] Name_Texts;
    [SerializeField] private Image[] Icon_Images;
    [SerializeField] private Text[] Des_Texts;
    [SerializeField] private Text[] Cost_Texts;
    [SerializeField] private Button[] UpIndex_Buttons;

    // ���� ��ũ��Ʈ
    [SerializeField] private Ply_Controller player_Con;
    [SerializeField] private Ply_Movement player_move;

    [SerializeField] private Image UI;
    [SerializeField] private Text Damage;
    [SerializeField] private Text HP;
    [SerializeField] private GameObject Upgrade1_Ob;
    [SerializeField] private GameObject Upgrade2_Ob;
    [SerializeField] private GameObject Content;
    [SerializeField] private Button[] buttons;
   

    private void OnEnable()
    {
        player_Con = FindObjectOfType<Ply_Controller>();
        player_move = FindObjectOfType<Ply_Movement>();
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
        if (GameManager.instance.Gold >= Data[0].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(0))
        {
            GameManager.instance.Gold -= Data[0].Upgrade_Cost;
            GameManager.instance.Max_MinionCount += (int)Data[0].Value;
            UpIndex_Buttons[0].interactable = false;
            GameManager.instance.Upgrade_List.Add(0);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            return;
        }
    }

    public void Upgrade_RespawnTime()
    {
        if (GameManager.instance.Gold >= Data[1].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(1))
        {
            GameManager.instance.Gold -= Data[1].Upgrade_Cost;
            GameManager.instance.respawnTime -= Data[1].Value;
            UpIndex_Buttons[1].interactable = false;
            GameManager.instance.Upgrade_List.Add(1);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Upgrade_Sol1()
    {
        if(GameManager.instance.Gold >= Data[2].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(2))
        {
            GameManager.instance.Gold -= Data[2].Upgrade_Cost;
            GameManager.instance.isPossible_Upgrade_1 = true;
            Upgrade1_Ob.SetActive(true);
            UpIndex_Buttons[2].interactable = false;
            GameManager.instance.Upgrade_List.Add(2);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Upgrade_Sol2()
    {
        if (GameManager.instance.Gold >= Data[3].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(3))
        {
            GameManager.instance.Gold -= Data[3].Upgrade_Cost;
            GameManager.instance.isPossible_Upgrade_2 = true;
            Upgrade2_Ob.SetActive(true);
            UpIndex_Buttons[3].interactable = false;
            GameManager.instance.Upgrade_List.Add(3);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Upgrade_LeaderATK()
    {
        if (GameManager.instance.Gold >= Data[4].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(4))
        {
            GameManager.instance.Gold -= Data[4].Upgrade_Cost;
            GameManager.instance.Damage += Data[4].Value;
            UpIndex_Buttons[4].interactable = false;
            GameManager.instance.Upgrade_List.Add(4);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Upgrade_LeaderHP()
    {
        if (GameManager.instance.Gold >= Data[5].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(5))
        {
            GameManager.instance.Gold -= Data[5].Upgrade_Cost;
            GameManager.instance.Max_Hp += Data[5].Value;
            GameManager.instance.Current_HP += Data[5].Value;
            UpIndex_Buttons[5].interactable = false;
            GameManager.instance.Upgrade_List.Add(5);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Upgrade_SolDAM()
    {
        if (GameManager.instance.Gold >= Data[6].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(6))
        {
            GameManager.instance.Gold -= Data[6].Upgrade_Cost;
            player_Con.isUpgrade_SolDam = true;
            UpIndex_Buttons[6].interactable = false;
            GameManager.instance.Upgrade_List.Add(6);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Upgrade_SolHP()
    {
        if (GameManager.instance.Gold >= Data[7].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(7))
        {
            GameManager.instance.Gold -= Data[7].Upgrade_Cost;
            player_Con.isUpgrade_SolHP = true;
            UpIndex_Buttons[7].interactable = false;
            GameManager.instance.Upgrade_List.Add(7);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    //Astar�ϼ��� �ٽ� �ۼ�
    public void Upgrade_Speed()
    {
        if (GameManager.instance.Gold >= Data[8].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(8))
        {
            GameManager.instance.Gold -= Data[8].Upgrade_Cost;
            player_move.MoveSpeed *= 1.2f;
            //���� �̵��ӵ� �߰� �ؾ���
            UpIndex_Buttons[8].interactable = false;
            GameManager.instance.Upgrade_List.Add(8);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }
  
    public void Upgrade_Income()
    {
        if (GameManager.instance.Gold >= Data[9].Upgrade_Cost && !GameManager.instance.Upgrade_List.Contains(9))
        {
            GameManager.instance.Gold -= Data[9].Upgrade_Cost;
            GameManager.instance.Upgrade_GoldValue = 1.1f;
            UpIndex_Buttons[9].interactable = false;
            GameManager.instance.Upgrade_List.Add(9);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

}
