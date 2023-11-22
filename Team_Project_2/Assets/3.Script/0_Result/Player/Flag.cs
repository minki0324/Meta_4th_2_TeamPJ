using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // 점령
    // 1. 점령 후 깃발 색 변경       

    public float Total_Gauge = 100f; // 전체 점령 게이지
    public float Current_Gauge = 0;  // 현재 점령 게이지
    private int TeamColor_Temp;

    public float Soldier_Multi = 1.03f; // 사람 수에 따른 배율
    public float occu_Speed = 12f; // 점령 속도

    public bool isOccupating = false; // 점령 중인지
    public bool isOccupied = false; // 점령이 끝났는지

    private EnemySpawn Spawn;
    public int Flag_Num;
    RaycastHit[] allHits;
    int Totalcount;

    List<(int, int)> TeamCount;

    [SerializeField] private SkinnedMeshRenderer skinnedmesh;
    public OccupationHUD OccuHUD;
    public List<int> Units = new List<int>();
    public int[] Value;
    private int Team1Minion = 0;
    private int Team2Minion = 0;
    int TeamColor;
    int scanRange = 8;
    public int Team1Count;
    public int Team2Count;
    public int Team3Count;
    public int Team4Count;

    public List<GameObject> Leaders = new List<GameObject>();

    public event Action isOccuDone;

    private void Start()
    {
        GameManager.instance.Flags.Add(gameObject.GetComponent<Flag>());
        OccuHUD = FindObjectOfType<OccupationHUD>();
        gameObject.layer = (transform.parent == null) ? 0 : ParentLayer();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0);

        Totalcount = allHits.Length;
        Team1Count = 0;
        Team2Count = 0;
        Team3Count = 0;
        Team4Count = 0;

        foreach (RaycastHit hit in allHits)
        {
            if (hit.transform.gameObject.CompareTag("SpawnPoint") || hit.transform.CompareTag("Flag") || hit.transform.CompareTag("Base") || hit.transform.gameObject.CompareTag("Weapon"))
            {
                continue;
            }
            
            int layer = hit.transform.gameObject.layer;
         
            switch (layer)
            {

                case 6:
                    Team1Count++;
                    break;
                case 7:
                    Team2Count++;
                    break;
                case 8:
                    Team3Count++;
                    break;
                case 9:
                    Team4Count++;
                    break;
            }
        }
        TeamCount = new List<(int , int)>();

        if (Team1Count > 0) TeamCount.Add((Team1Count, 6));
        if (Team2Count > 0) TeamCount.Add((Team2Count, 7));
        if (Team3Count > 0) TeamCount.Add((Team3Count, 8));
        if (Team4Count > 0) TeamCount.Add((Team4Count, 9));

        if (!isOccupating)
        {
            switch (TeamCount.Count)
            {
                case 1:
                    if (TeamCount[0].Item2 != gameObject.layer)
                    {
                        switch (TeamCount[0].Item2)
                        {
                            case (int)TeamLayerIdx.Player:
                                TeamColor = GameManager.instance.Color_Index;
                                break;
                            case (int)TeamLayerIdx.Team1:
                                TeamColor = GameManager.instance.T1_Color;
                                break;
                            case (int)TeamLayerIdx.Team2:
                                TeamColor = GameManager.instance.T2_Color;
                                break;
                            case (int)TeamLayerIdx.Team3:
                                TeamColor = GameManager.instance.T3_Color;
                                break;
                        }
                        StartCoroutine(OnOccu_co(TeamColor, TeamCount[0].Item1, TeamCount[0].Item2));
                    }
                    break;
                case 2:
                    if (Team1Minion.Equals(Team2Minion)) return;
                    int CurrentMinion = TeamCount[0].Item1 > TeamCount[1].Item1 ? TeamCount[0].Item1 : TeamCount[1].Item1;
                    TeamColor = TeamCount[0].Item1 > TeamCount[1].Item1 ? TeamCount[0].Item2 : TeamCount[1].Item2;
                    if (!Value[0].Equals(gameObject.layer))
                    {
                        switch (TeamColor)
                        {
                            case (int)TeamLayerIdx.Player:
                                TeamColor = GameManager.instance.Color_Index;
                                break;
                            case (int)TeamLayerIdx.Team1:
                                TeamColor = GameManager.instance.T1_Color;
                                break;
                            case (int)TeamLayerIdx.Team2:
                                TeamColor = GameManager.instance.T2_Color;
                                break;
                            case (int)TeamLayerIdx.Team3:
                                TeamColor = GameManager.instance.T3_Color;
                                break;
                        }
                        StartCoroutine(OnOccu_co(TeamColor, CurrentMinion, TeamCount[0].Item2));
                    }
                    break;
                default:
                    return;
            }
        }
    }




    public void Change_Flag_Color(int TeamNum)
    {
        skinnedmesh.material = ColorManager.instance.Flag_Color[TeamNum];
    }
    
    private int ParentLayer()
    {
        return this.transform.parent.gameObject.layer;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.isLive)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OccuHUD.Ply_OccuHUD(Flag_Num, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.isLive)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OccuHUD.Ply_OccuHUD(Flag_Num, false);
            }        
        }
    }

    public IEnumerator OnOccu_co(int TeamColor, int Current_Minion, int Teamlayer)
    {
        // Case1 다른진영 -> 중립 / 중립 -> 본인진영
        // Case2 중립 -> 본인진영
        isOccupating = true;
        while (isOccupied && isOccupating && Current_Gauge >= 0f && !Teamlayer.Equals(gameObject.layer)) 
        {
            Current_Gauge -= Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, Current_Minion);
            OccuHUD.Ply_Slider(TeamColor_Temp, Flag_Num, Current_Gauge, Total_Gauge);
            yield return null;
        }

        if (Current_Gauge <= 0f && isOccupied)
        {
            isOccupied = false;  
            OccuHUD.Ply_Slider((int)ColorIdx.White, Flag_Num,Current_Gauge,Total_Gauge);
            OccuHUD.Change_Color((int)ColorIdx.White, Flag_Num);

            gameObject.layer = 0;
            GameManager.instance.Set_FlagCount();
        }

        // 중립지역을 점령할 때
        while (!isOccupied && isOccupating && Current_Gauge <= Total_Gauge) 
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, Current_Minion); 
            OccuHUD.Ply_Slider(TeamColor,Flag_Num, Current_Gauge, Total_Gauge);
            yield return null;
        }

        if (Current_Gauge >= Total_Gauge && !isOccupied)
        {
            isOccupied = true;   // 중립 -> 본인진영
            TeamColor_Temp = TeamColor;
            gameObject.layer = Teamlayer;
            OccuHUD.Change_Color(TeamColor, Flag_Num);
            ColorManager.instance.Change_SolidColor(transform.GetChild(1).GetComponent<SpriteRenderer>(), TeamColor);
            GameManager.instance.Set_FlagCount();
        }

        isOccupating = false;
        
        yield return null;
    }
    public IEnumerator OffOccu_co()
    {
        yield return new WaitForSeconds(3.0f);

        // 점령된 곳에서 점령하다가 나왔을 때
        while (isOccupied && !isOccupating && Current_Gauge <= 100f)
        {           
            Current_Gauge += Time.deltaTime * occu_Speed;
            yield return null;
        }
        // 중립에서 점령 하다가 나갔을 때
        while (!isOccupied && !isOccupating && Current_Gauge >= 0f) 
        {            
            Current_Gauge -= Time.deltaTime * occu_Speed;
            yield return null;
        }
      
      
        yield return null;
    }




}
