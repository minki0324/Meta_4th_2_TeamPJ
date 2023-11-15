using System.Collections;
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

    private SkinnedMeshRenderer skinnedmesh;
    public Unit_Occupation unit_O;
    private OccupationHUD OccuHUD;

    private void Awake()
    {
        OccuHUD = FindObjectOfType<OccupationHUD>();
        TryGetComponent<SkinnedMeshRenderer>(out skinnedmesh);
    }

    private void Start()
    {       
        transform.parent.gameObject.layer = transform.root.gameObject.layer;
    }

    public void Change_Flag_Color(int TeamNum)
    { 
        skinnedmesh.material = ColorManager.instance.Flag_Color[TeamNum];        
    }

    private int ParentLayer()
    {
        return this.transform.parent.gameObject.layer;
    }
    private int UnitLayer(Unit_Occupation unit)
    {
        return unit.gameObject.layer;
    }


    public IEnumerator OnOccu_co(int TeamColor, int Teamlayer)
    {
        // Case1 다른진영 -> 중립 / 중립 -> 본인진영
        // Case2 중립 -> 본인진영

        // 점령 중
        if (Teamlayer.Equals(TeamLayerIdx.Player))
        {
            unit_O.OccuHUD.Ply_OccuHUD(unit_O.Flag_Num, true);
        }

        // 점령지역을 점령할 때
        while (isOccupied && isOccupating && Current_Gauge >= 0f && !ParentLayer().Equals(UnitLayer(unit_O))) 
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, 20); // 나중에 인원수에 따른 배율 넣어야해용
            OccuHUD.Ply_Slider(TeamColor_Temp, unit_O.Flag_Num, Current_Gauge, Total_Gauge);
            Debug.Log(Current_Gauge);
            yield return null;
        }

        if (Current_Gauge <= 0f && isOccupied)
        {
            isOccupied = false;   // 상대진영 -> 중립
            OccuHUD.Ply_Slider((int)ColorIdx.White, unit_O.Flag_Num,Current_Gauge,Total_Gauge);
            OccuHUD.Change_Color((int)ColorIdx.White, unit_O.Flag_Num);

            this.transform.parent.gameObject.layer = 0;
        }

        // 중립지역을 점령할 때
        while (!isOccupied && isOccupating && Current_Gauge <= Total_Gauge) 
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, 20); // 나중에 인원수에 따른 배율 넣어야해용
            OccuHUD.Ply_Slider(TeamColor, unit_O.Flag_Num, Current_Gauge, Total_Gauge);
            Debug.Log(Current_Gauge);
            yield return null;
        }

        if (Current_Gauge >= Total_Gauge && !isOccupied)
        {
            isOccupied = true;   // 중립 -> 본인진영
            TeamColor_Temp = TeamColor;
            unit_O.OccuHUD.Change_Color(TeamColor, unit_O.Flag_Num);
            this.transform.parent.gameObject.layer = UnitLayer(unit_O);
        }


        
        yield return null;
    }
    public IEnumerator OffOccu_co(int Teamlayer)
    {
        if (Teamlayer.Equals((int)TeamLayerIdx.Player))
        {
            unit_O.OccuHUD.Ply_OccuHUD(unit_O.Flag_Num, false);
        }
        yield return new WaitForSeconds(3.0f);

        // 점령된 곳에서 점령하다가 나왔을 때
        while (isOccupied && !isOccupating && Current_Gauge <= 100f)
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, 20);
        }
        // 중립에서 점령 하다가 나갔을 때
        while (!isOccupied && !isOccupating && Current_Gauge >= 0f) 
        {
            Current_Gauge -= Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, 20);
            yield return null;
        }
        
        
        yield return null;
    }


}
