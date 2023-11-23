using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public DataManager Json;
    public PlayerData Ply_Data;
    public bool isFreeze = false;

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
    public float EndTime = 20f;   // 게임 시간은 30분
    public int Occupied_Area = 1;   // 점령한 지역 Default값 1
    public GameObject Result;
    public Text txt;
    public AudioSource MainBgm;
    public AudioSource WeatherSFX;

    bool isColorSame = false;
    public bool isGameEnd = false;
    

    [Header("골드 관련")]
    public float total_Gold = 1000;
    public float Gold = 1000;       // 골드량
    private float Magnifi = 2f;     // 기본 골드 배율 (업데이트문 프레임 60 x 2f로 기본 획득 골드량은 분당 120)
    public float Upgrade_GoldValue = 1f;
    
    [Header("플레이어 관련")]
    public bool isLive = false;
    public bool isDead;
    public bool inRange;
    private bool isFastMode;
    public float Current_HP = 150f;
    public float Max_Hp = 150f;
    public float Damage = 20f; 
    public float Regeneration = 0.5f;
    public float respawnTime = 10f;
    public int killCount;
    public int DeathCount;
    public int Ply_hasFlag = 1;
    public float Teampoint = 0;
    public int Hire = 0;

    //병사인구 
    public int Max_MinionCount = 19;
    public int Current_MinionCount;
    //병종 업그레이드
    public bool isPossible_Upgrade_1 = false;
    public bool isPossible_Upgrade_2 = false;
    public List<int> Upgrade_List = new List<int>();

    //스크립터블 배열
    [Header("Sword > Heavy > Archer > Priest > Spear > Halberdier ")]
    public Unit_Information[] units;
    public Unit_Information unit0;
    public Unit_Information unit1;
    public Unit_Information unit2;

    public List<LeaderState> leaders;
    public List<Flag> Flags;

    [Header("컬러인덱스")]
    public int Color_Index;         // 플레이어 컬러 인덱스
    public int T1_Color;
    public int T2_Color;
    public int T3_Color;

    [Header("업그레이드")]
    [SerializeField] private Enemy_Upgrade upgrade;
    private float Upgrade_Time;

    //그래프 관련
    public List<float> TeamPoint_graph;
    public List<float> EnemyPoint_graph_1;
    public List<float> EnemyPoint_graph_2;
    public List<float> EnemyPoint_graph_3;
    public float current_Time = 0;


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
        Json = GetComponent<DataManager>();
    }
    private void Start()
    {
        MainBgm.clip = AudioManager.instance.clip_BGM[(int)BGMList.MainBGM];
        MainBgm.Play();
        WeatherSFX.clip = AudioManager.instance.clip_SFX[(int)SFXList.Wind_Storm];
        WeatherSFX.Play();
        
    }

    // 기존 골드 상승량
    // 점령 어드벤티지
    // 골드 상승량 업그레이드

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!isFreeze)
            {
                Stop();
                isFreeze = !isFreeze;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (isFreeze)
            {
                Resume();
                isFreeze = !isFreeze;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        if (!isLive)
        {
            return;
        }
        
        currentTime += Time.deltaTime;
        Upgrade_Time += Time.deltaTime;
        total_Gold += Time.deltaTime * Magnifi * Ply_hasFlag * Upgrade_GoldValue;
        Gold += Time.deltaTime * Magnifi * Ply_hasFlag * Upgrade_GoldValue; // 골드수급 = 분당 120 * 점령한 지역 개수

        if(Input.GetKeyDown(KeyCode.K))
        {
            isFastMode = !isFastMode;
            if (isFastMode) { 
            Time.timeScale = 4.5f;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (current_Time + (EndTime / 20) <= currentTime)
        {
            current_Time += (EndTime / 20);
            TeamPoint_graph.Add(Teampoint);
            EnemyPoint_graph_1.Add(leaders[0].Teampoint);
            EnemyPoint_graph_2.Add(leaders[1].Teampoint);
            EnemyPoint_graph_3.Add(leaders[2].Teampoint);
            
        }





        //게임 종료 관련
        if (currentTime >= EndTime)
        {
            isGameEnd = true;
        }
        if (Ply_hasFlag <= 0)
        {
            isGameEnd = true;
        }
        EndGame();
        Upgrade();

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


    public EnemySpawn FindSpawnPoint(GameObject leader, int layer)
    {
        EnemySpawn spawnPoint =null;
        GameObject[] spawns;
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        float mindistance = float.MaxValue;
        foreach (GameObject ob in spawns)
        {
            if (layer == ob.gameObject.layer)
            {

                float distance = Vector3.Distance(leader.transform.position, ob.transform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    spawnPoint = ob.GetComponent<EnemySpawn>();

                }
            }
        }
        if(spawnPoint == null)
        {
            Debug.Log("스폰포인트 없음");
        }

        return spawnPoint;
    }

    public void Save_N_BackToMain()
    {
        Json.Save_playerData(PlayerID, PlayerCoin, isCanUse_SwordMan, isCanUse_Knight, isCanUse_Archer, isCanUse_SpearMan, isCanUse_Halberdier, isCanUse_Prist);
        SceneManager.LoadScene(0);
    }

    public void Set_FlagCount()
    {
        int flagCount1 = 0;
        int flagCount2 = 0;
        int flagCount3 = 0;
        int flagCount4 = 0;
        for (int i = 0; i < Flags.Count; i++)
        {
            switch (Flags[i].gameObject.layer)
            {
                case 6:
                    flagCount1++;
                    break;
                case 7:
                    flagCount2++;
                    break;
                case 8:
                    flagCount3++;
                    break;
                case 9:
                    flagCount4++;
                    break;
            }
        }
        Ply_hasFlag = flagCount1;
        leaders[0].has_Flag = flagCount2;
        leaders[1].has_Flag = flagCount3;
        leaders[2].has_Flag = flagCount4;

    }


    public void EndGame()
    {
        if (isGameEnd)
        {
            Stop();
            PlayerCoin = PlayerCoin + (int)Teampoint / 1000;
           
            Result.SetActive(true);
            GetwinnerTeam();
        }
    }


    //모든 깃발이 한팀에 점령 당했을 경우
    public void IsAllFlagOccupied()
    {
        
        //모든 깃발색이 맞나 검사
        for (int i = 0; i < Flags.Count - 1; i++)
        {
            if (Flags[i].gameObject.layer == Flags[i + 1].gameObject.layer)
            {
                isColorSame = true;
            }
            else
            {
                isColorSame = false;
            }
        }

        if(isColorSame)
        {
            isGameEnd = true;
        }
        
      
    }

    //승리팀 판단
    public void GetwinnerTeam()
    {
        if (isGameEnd)
        {
            for (int i = 0; i < leaders.Count; i++)
            {
                if (Teampoint < leaders[i].Teampoint)
                {
                    txt.text = "Player Lose..";
                }
                else
                {
                    txt.text = "Player WIN!!!";
                }
            }

        }
    }

    public void Upgrade()
    {
        if(Upgrade_Time > 180)
        {
            for(int i = 0; i < leaders.Count; i++)
            {
                while(true)
                {
                    try
                    {
                        upgrade.Upgradeall(i, Random.Range(0, 9));
                        break;
                    }
                    catch
                    {
                        //upgrade.Upgradeall(i, Random.Range(0, 9));
                    }
                }
                Debug.Log(i + "번째 업글");
            }
            Upgrade_Time = 0;
        }
    }
    


}
