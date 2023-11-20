using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderState : Unit
{

    public enum BattleState
    {

        //Follow, //점령지 이동할때 
        Attack,  // AI가 적을 인지하고 일정시간 또는 거리가됬을때 
        Wait,
        Move, //상체 아이들상태로 뛰어가기
        Defense,
        Detect //방패들기
        

    }
    public float Teampoint = 0;

    [Header("골드 관련")]
    public float total_Gold = 500;
    public float Gold = 500; // 골드량
    public float Upgrade_GoldValue = 1f;
    // private float Magnifi = 2f;  // 기본 골드 배율 (업데이트문 프레임 60 x 2f로 기본 획득 골드량은 분당 120)

    [Header("AI 관련")]
    // private bool Ready =true;
    public float Regeneration = 0.5f;
    public int maxUnitCount = 19;
    public int currentUnitCount = 0;
    public int killCount = 0;
    public int deathCount = 0;
    public int unitValue = 0;
    public float unitCost = 16f;
    public bool canSpawn;
    public bool isMoving;
    public Transform respawnPoint;
    public int Team_Color;
    public int has_Flag = 0;
    //EnemySpawn respawnPoint;
    public BattleState bat_State;
    public int Hire = 0;
    public bool isPossible_Upgrade_1 = false;
    public bool isPossible_Upgrade_2 = false;

    public bool isUpgrade_SolDam = false;
    public bool isUpgrade_SolHP = false;
    public List<int> Upgrade_List = new List<int>();
    public List<GameObject> UnitList = new List<GameObject>();


    private void Start()
    {
        switch (this.gameObject.layer)
        {
            case (int)TeamLayerIdx.Player:
                Team_Color = GameManager.instance.Color_Index;
                break;
            case (int)TeamLayerIdx.Team1:
                Team_Color = GameManager.instance.T1_Color;
                break;
            case (int)TeamLayerIdx.Team2:
                Team_Color = GameManager.instance.T2_Color;
                break;
            case (int)TeamLayerIdx.Team3:
                Team_Color = GameManager.instance.T3_Color;
                break;
            default:
                return;
        }
    }


    public override void Die()
    {
     
    }
    //AI 행동 우선순위
    /*
     1. 중립지역이 있을때
     2. 중립지역이 아니지만 아무도 없을때 
     3. 
     
     1.전투
     2. 아무도없을시 점령
     
     */
   
    public override void HitDamage(float damage)
    {
       
    }
    public override void Lostleader()
    {
       
    }
}
