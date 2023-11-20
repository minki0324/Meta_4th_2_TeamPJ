using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        ���� �Ŵ������� �����ؾ� �� ���� ���
        1. ���
        2. �÷��̾� ü��
        3. ������ (���� ����)
    */
    public static GameManager instance = null;
    public DataManager Json;
    public PlayerData Ply_Data;

    [Header("���� ���")]
    public int GameMode = 0;
    public int GameSpeed = 0;

    [SerializeField] private GameObject Option;
    [Header("���� ����")]
    public string PlayerID;
    public int PlayerCoin;
    public bool isCanUse_SwordMan;
    public bool isCanUse_Knight;
    public bool isCanUse_Archer;
    public bool isCanUse_SpearMan;
    public bool isCanUse_Halberdier;
    public bool isCanUse_Prist;

    [Header("���� �÷���")]
    public float currentTime = 0f;  // ������ �����ϰ� ���� �ð�
    public float EndTime = 20f;   // ���� �ð��� 30��
    public int Occupied_Area = 1;   // ������ ���� Default�� 1
    public GameObject Result;
    public AudioClip MainBgm;

    [Header("��� ����")]
    public float total_Gold = 1000;
    public float Gold = 1000;       // ��差
    private float Magnifi = 2f;     // �⺻ ��� ���� (������Ʈ�� ������ 60 x 2f�� �⺻ ȹ�� ��差�� �д� 120)
    public float Upgrade_GoldValue = 1f;
    
    [Header("�÷��̾� ����")]
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
    public int Ply_hasFlag = 0;
    public float Teampoint = 0;
    public int Hire = 0;

    //�����α� 
    public int Max_MinionCount = 19;
    public int Current_MinionCount;
    //���� ���׷��̵�
    public bool isPossible_Upgrade_1 = false;
    public bool isPossible_Upgrade_2 = false;
    public List<int> Upgrade_List = new List<int>();

    //��ũ���ͺ� �迭
    [Header("Sword > Heavy > Archer > Priest > Spear > Halberdier ")]
    public Unit_Information[] units;
    public Unit_Information unit0;
    public Unit_Information unit1;
    public Unit_Information unit2;

    public List<LeaderState> leaders;

    [Header("�÷��ε���")]
    public int Color_Index;         // �÷��̾� �÷� �ε���
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
        Json = GetComponent<DataManager>();
    }
   
    // ���� ��� ��·�
    // ���� ��庥Ƽ��
    // ��� ��·� ���׷��̵�
    
    private void Update()
    {
       
        if(!isLive)
        {
            return;
        }
        
        currentTime += Time.deltaTime;

        total_Gold += Time.deltaTime * Magnifi * Occupied_Area * Upgrade_GoldValue;
        Gold += Time.deltaTime * Magnifi * Occupied_Area * Upgrade_GoldValue; // ������ = �д� 120 * ������ ���� ����

        if(Input.GetKeyDown(KeyCode.K))
        {
            isFastMode = !isFastMode;
            if (isFastMode) { 
            Time.timeScale = 10f;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
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

    public void Save_N_BackToMain()
    {
        Json.Save_playerData(PlayerID, PlayerCoin, isCanUse_SwordMan, isCanUse_Knight, isCanUse_Archer, isCanUse_SpearMan, isCanUse_Halberdier, isCanUse_Prist);
        SceneManager.LoadScene(0);
    }
    
    public void EndGame()
    {
        Stop();
        PlayerCoin = PlayerCoin + (int)Teampoint / 1000;
        Result.SetActive(true);

    }
}
