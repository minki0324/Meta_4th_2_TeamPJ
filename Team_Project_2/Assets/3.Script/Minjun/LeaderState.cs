using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderState : MonoBehaviour
{

    public enum BattleState {
    
        Follow, //점령지 이동할때 
        Attack,  // AI가 적을 인지하고 일정시간 또는 거리가됬을때 
        Detect,
        
    }
    public enum JudgmentState
    {
        Ready, //평균 전투력보다 높으면 점령지 이동
        Wait, //평균전투력보다 낮을때 기지에서 대기하며 애들뽑기
        Going

    }
    public enum AniState 
    {
        Idle,
        Attack,
        shild,
        Order
    }





    [Header("골드 관련")]
    public float Gold = 500; // 골드량
    // private float Magnifi = 2f;  // 기본 골드 배율 (업데이트문 프레임 60 x 2f로 기본 획득 골드량은 분당 120)

    [Header("AI 관련")]
    public bool isLive = true;
    // private bool Ready =true;
    public float Current_HP = 150f;
    public float Max_Hp = 150f;
    public float Regeneration = 0.5f;
    public int maxUnitCount = 19;
    public int currentUnitCount = 0;
    public int unitValue = 0;
    public float unitCost =16f;
    public bool canSpawn;
    public bool isDead;
    public bool isMoving;
    public BattleState bat_State;
    public JudgmentState jud_State;
    public AniState ani_State;

    public List<GameObject> UnitList = new List<GameObject>();

  
    //AI 행동 우선순위
    /*
     1. 중립지역이 있을때
     2. 중립지역이 아니지만 아무도 없을때 
     3. 
     
     1.전투
     2. 아무도없을시 점령
     
     */

}
