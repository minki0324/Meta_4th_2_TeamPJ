using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamLayerIdx
{
    Player = 6,
    Team1,
    Team2,
    Team3
}

public class GameManager : MonoBehaviour
{
    /*
        게임 매니저에서 관리해야 할 변수 목록
        1. 골드
        2. 플레이어 체력
        3. 점령지 (골드와 연동)
    */
    public static GameManager instance = null;

    [Header("게임 모드")]
    public int GameMode = 0;
    public int GameSpeed = 0;

    [SerializeField] private GameObject Option;
    [Header("계정 관련")]
    public string PlayerID;
    public int PlayerCoin;
    public bool isCanUse_SwordMan;
    public bool isCanUse_Knight;
    public bool isCanUse_Archer;
    public bool isCanUse_SpearMan;
    public bool isCanUse_Halberdier;
    public bool isCanUse_Prist;

    [Header("게임 플레이")]
    public float currentTime = 0f;  // 게임이 시작하고 지난 시간
    public float EndTime = 1800f;   // 게임 시간은 30분
    public int Occupied_Area = 1;   // 점령한 지역 Default값 1

    [Header("골드 관련")]
    public float total_Gold = 1000;
    public float Gold = 1000;       // 골드량
    private float Magnifi = 2f;     // 기본 골드 배율 (업데이트문 프레임 60 x 2f로 기본 획득 골드량은 분당 120)
    
    [Header("플레이어 관련")]
    public bool isLive = false;
    public bool isDead;
    public bool inRange;
    public float Current_HP = 150f;
    public float Max_Hp = 150f;
    public float Damage = 20f; 
    public float Regeneration = 0.5f;
    public float respawnTime = 10f;
    public int killCount;
    public int DeathCount;
    public int Ply_hasFlag = 0;
    public float Teampoint = 0;
    public int Hire = 0;

    //병사인구 
    public int Max_MinionCount = 19;
    public int Current_MinionCount;
    //병종 업그레이드
    public bool isPossible_Upgrade_1 = false;
    public bool isPossible_Upgrade_2 = false;
    //스크립터블 배열
    [Header("Sword > Heavy > Archer > Priest > Spear > Halberdier ")]
    public Unit_Information[] units;
    public Unit_Information unit0;
    public Unit_Information unit1;
    public Unit_Information unit2;

    public List<LeaderState> leaders;

    [Header("컬러인덱스")]
    public int Color_Index;         // 플레이어 컬러 인덱스
    public int T1_Color;
    public int T2_Color;
    public int T3_Color;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    // 기존 골드 상승량
    // 점령 어드벤티지
    // 골드 상승량 업그레이드
    
    private void Update()
    {
       
        if(!isLive)
        {
            return;
        }
        
        currentTime += Time.deltaTime;

        total_Gold += Time.deltaTime * Magnifi * Occupied_Area;
        Gold += Time.deltaTime * Magnifi * Occupied_Area; // 골드수급 = 분당 120 * 점령한 지역 개수
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }


    public EnemySpawn FindSpawnPoint(GameObject leader)
    {
        EnemySpawn spawnPoint =null;
        GameObject[] spawns;
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < spawns.Length; i++)
        {

        Debug.Log(spawns[i]);   
        }
        float mindistance = float.MaxValue;
        foreach (GameObject ob in spawns)
        {
            if (leader.layer == ob.gameObject.layer)
            {

                float distance = Vector3.Distance(leader.transform.position, ob.transform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    spawnPoint = ob.GetComponent<EnemySpawn>();

                }
            }
        }

        return spawnPoint;
    }
  
}
