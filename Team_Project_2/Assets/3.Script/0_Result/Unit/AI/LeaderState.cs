using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderState : Unit
{

    public enum BattleState
    {

        //Follow, //점령지 이동할때 
        Attack,  // AI가 적을 인지하고 일정시간 또는 거리가됬을때 
        Search,
        Move,
        Defense,
        Detect


    }
    public float Teampoint = 0;

    [Header("골드 관련")]
    public float total_Gold = 500;
    public float Gold = 500; // 골드량
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
    //EnemySpawn respawnPoint;
    public BattleState bat_State;
    public int has_Flag = 0;
    public int Hire = 0;

    public List<GameObject> UnitList = new List<GameObject>();

   
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
    public void Respawn(GameObject leader)
    {
        //애니메이션초기화
        //HP , 콜라이더 , isDead ,레이어 다시설정
        //저장한 리스폰 위치로 이동
        aipath.canMove = false;
        aipath.canSearch = false;
        ani.SetTrigger("Reset");
        ani.SetLayerWeight(1, 1);
        data.currentHP = data.maxHP;
        data.isDie = false;
        Debug.Log(respawnPoint.parent.gameObject.layer);
        leader.layer = respawnPoint.parent.gameObject.layer;
        leader.transform.position = respawnPoint.position;


    }
    public override void HitDamage(float damage)
    {
       
    }
    public override void Lostleader()
    {
       
    }
}
