using System.Collections;
using System.Linq;
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

    public int Flag_Num;



    [SerializeField] private SkinnedMeshRenderer skinnedmesh;
    private OccupationHUD OccuHUD;
    private List<int> Units = new List<int>();
    public int[] Value;
    private int Team1Minion = 0;
    private int Team2Minion = 0;




    private void Start()
    {
        OccuHUD = FindObjectOfType<OccupationHUD>();
        gameObject.layer = (transform.parent == null) ? 0 : ParentLayer();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (Units.Count.Equals(0))
        {
           
        }

        if (Units.Count > 0 && !isOccupating) // 병사가 들어있긴 함..
        {
            Value = Units.Distinct<int>().ToArray<int>();
            switch (Value.Length)
            {
                case 1:
                    StartCoroutine(OnOccu_co(Value[0], Units.Count));
                    break;
                case 2:
                    for (int i = 0; i < Units.Count; i++)
                    {
                        if (Units[i].Equals(Value[0])) Team1Minion++;
                        else Team2Minion++;
                    }
                    if (Team1Minion.Equals(Team2Minion)) return;
                    int CurrentMinion = (Team1Minion > Team2Minion) ? Team1Minion - Team2Minion : Team2Minion - Team1Minion;
                    int TeamColor = (Team1Minion > Team2Minion) ? Value[0] : Value[1];

                    switch(TeamColor)
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

                    StartCoroutine(OnOccu_co(TeamColor, CurrentMinion));            
                    break;
                default:
                    return;
            }
        }

        Totalcount = allHits.Length;
        Team1Count = 0;
        Team2Count = 0;
        Team3Count = 0;
        Team4Count = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.isLive)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OccuHUD.Ply_OccuHUD(Flag_Num, true);
            }
            Units.Add(other.gameObject.layer);

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
            Units.Remove(other.gameObject.layer);
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



    public IEnumerator OnOccu_co(int TeamColor, int Current_Minion)
    {
        // Case1 다른진영 -> 중립 / 중립 -> 본인진영
        // Case2 중립 -> 본인진영
        isOccupating = true;
        while (isOccupied && isOccupating && Current_Gauge >= 0f) 
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, Current_Minion);
            OccuHUD.Ply_Slider(TeamColor_Temp, Flag_Num, Current_Gauge, Total_Gauge);
            yield return null;
        }

        if (Current_Gauge <= 0f && isOccupied)
        {
            isOccupied = false;  
            OccuHUD.Ply_Slider((int)ColorIdx.White, Flag_Num,Current_Gauge,Total_Gauge);
            OccuHUD.Change_Color((int)ColorIdx.White, Flag_Num);

            this.transform.parent.gameObject.layer = 0;
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



    private void Triggercol(Collider other, bool isComein)
    {

    }


}
