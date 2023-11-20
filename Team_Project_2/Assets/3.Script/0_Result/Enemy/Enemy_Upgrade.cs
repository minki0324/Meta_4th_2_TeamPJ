using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Upgrade : MonoBehaviour
{
    [SerializeField] private UpgradeData[] Data;
      
    public void Upgradeall(int Team, int upgradeidx)
    {
        if (upgradeidx.Equals(0)) Upgrade_MaxCountUp(Team);
        else if (upgradeidx.Equals(1)) Upgrade_RespawnTime(Team);
        else if (upgradeidx.Equals(2)) Upgrade_Sol1(Team);
        else if (upgradeidx.Equals(3)) Upgrade_Sol2(Team);
        else if (upgradeidx.Equals(4)) Upgrade_LeaderATK(Team);
        else if (upgradeidx.Equals(5)) Upgrade_LeaderHP(Team);
        else if (upgradeidx.Equals(6)) Upgrade_SolDAM(Team);
        else if (upgradeidx.Equals(7)) Upgrade_SolHP(Team);
        else if (upgradeidx.Equals(8)) Upgrade_Speed(Team);
        else if (upgradeidx.Equals(9)) Upgrade_Income(Team);
    }

    // 인덱스 0번
    private void Upgrade_MaxCountUp(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[0].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(0))
        {
            GameManager.instance.leaders[Team].Gold -= Data[0].Upgrade_Cost;
            GameManager.instance.leaders[Team].maxUnitCount = GameManager.instance.leaders[Team].maxUnitCount + (int)Data[0].Value;
            GameManager.instance.leaders[Team].Upgrade_List.Add(0);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
            return;
        }
    }

    // 인덱스 1번
    private void Upgrade_RespawnTime(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[1].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(1))
        {
            GameManager.instance.leaders[Team].Gold -= Data[1].Upgrade_Cost;
            GameManager.instance.respawnTime = GameManager.instance.respawnTime - Data[1].Value;
            GameManager.instance.leaders[Team].Upgrade_List.Add(1);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
            return;
        }
    }

    private void Upgrade_Sol1(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[2].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(2))
        {
            GameManager.instance.leaders[Team].Gold -= Data[2].Upgrade_Cost;
            GameManager.instance.leaders[Team].isPossible_Upgrade_1 = true;
            GameManager.instance.leaders[Team].Upgrade_List.Add(2);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
            return;
        }
    }

    private void Upgrade_Sol2(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[3].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(3))
        {
            GameManager.instance.leaders[Team].Gold -= Data[3].Upgrade_Cost;
            GameManager.instance.leaders[Team].isPossible_Upgrade_2 = true;
            GameManager.instance.leaders[Team].Upgrade_List.Add(3);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
            return;
        }
    }

    private void Upgrade_LeaderATK(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[4].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(4))
        {
            GameManager.instance.leaders[Team].Gold -= Data[4].Upgrade_Cost;
            GameManager.instance.leaders[Team].data.damage = GameManager.instance.leaders[Team].data.damage + Data[4].Value;
            GameManager.instance.leaders[Team].Upgrade_List.Add(4);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
        }
    }

    private void Upgrade_LeaderHP(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[5].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(5))
        {
            GameManager.instance.leaders[Team].Gold -= Data[5].Upgrade_Cost;
            GameManager.instance.leaders[Team].data.maxHP = GameManager.instance.leaders[Team].data.maxHP + Data[5].Value;
            GameManager.instance.leaders[Team].data.currentHP = GameManager.instance.leaders[Team].data.currentHP + Data[5].Value;
            GameManager.instance.leaders[Team].Upgrade_List.Add(5);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
        }
    }

    private void Upgrade_SolDAM(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[6].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(6))
        {
            GameManager.instance.leaders[Team].Gold -= Data[6].Upgrade_Cost;
            GameManager.instance.leaders[Team].isUpgrade_SolDam = true;
            GameManager.instance.leaders[Team].Upgrade_List.Add(6);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
        }
    }

    private void Upgrade_SolHP(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[7].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(7))
        {
            GameManager.instance.leaders[Team].Gold -= Data[7].Upgrade_Cost;
            GameManager.instance.leaders[Team].isUpgrade_SolHP = true;
            GameManager.instance.leaders[Team].Upgrade_List.Add(7);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
        }
    }

    //Astar완성후 다시 작성
    private void Upgrade_Speed(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[8].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(8))
        {
            GameManager.instance.leaders[Team].Gold -= Data[8].Upgrade_Cost;
            // 머지 후 속도 변수 추가해서 담아주기
            /*GameManager.instance.leaders[Team].Speed *= 1.2f;*/
            // 병사 이동속도 추가 해야함
            GameManager.instance.leaders[Team].Upgrade_List.Add(8);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
        }
    }

    private void Upgrade_Income(int Team)
    {
        if (GameManager.instance.leaders[Team].Gold >= Data[9].Upgrade_Cost && !GameManager.instance.leaders[Team].Upgrade_List.Contains(9))
        {
            GameManager.instance.leaders[Team].Gold -= Data[9].Upgrade_Cost;
            GameManager.instance.leaders[Team].Upgrade_GoldValue = 1.1f;
            GameManager.instance.leaders[Team].Upgrade_List.Add(9);
        }
        else
        {
            Debug.Log("적군의 업그레이드 골드가 부족합니다.");
        }
    }
}
