using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // 점령
    // 1. 점령 후 깃발 색 변경       

    public float Total_Gauge = 100f; // 전체 점령 게이지
    public float Current_Gauge = 0;  // 현재 점령 게이지

    public float Soldier_Multi = 1.03f; // 사람 수에 따른 배율
    public float occu_Speed = 12f; // 점령 속도

    public bool isOccupating = false; // 점령 중인지
    public bool isOccupied = false; // 점령이 끝났는지

    private SkinnedMeshRenderer skinnedmesh;
    public Unit_Occupation unit_O;
    OccupationHUD OccuHUD;

    private void Awake()
    {
        OccuHUD = FindObjectOfType<OccupationHUD>();
        TryGetComponent<SkinnedMeshRenderer>(out skinnedmesh);
    }
    public void Change_Flag_Color(int TeamNum)
    { 
        skinnedmesh.material = ColorManager.instance.Flag_Color[TeamNum];        
    }
    private void Update()
    {
        if (Current_Gauge / Total_Gauge >= 1 && !isOccupied)
        {
            isOccupied = true;
        }

    }
    
    public IEnumerator OnOccu_co()
    {
        // 점령 중
        unit_O.OccuHUD.Ply_OccuHUD(unit_O.Flag_Num, true);

        while (isOccupating && Current_Gauge <= 100f)
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Mathf.Pow(Soldier_Multi, 20); // 나중에 인원수에 따른 배율 넣어야해용
            OccuHUD.Ply_Slider(6, unit_O.Flag_Num, Current_Gauge, Total_Gauge);
            Debug.Log(Current_Gauge);
            yield return null;
        }
        isOccupied = true;
        unit_O.OccuHUD.Change_Color(6, unit_O.Flag_Num);

        this.transform.parent.gameObject.layer = unit_O.gameObject.layer;
        yield return null;
    }
    public IEnumerator OffOccu_co()
    {
        unit_O.OccuHUD.Ply_OccuHUD(unit_O.Flag_Num, false);
        yield return new WaitForSeconds(3.0f);

        while (!isOccupied && !isOccupating && Current_Gauge >= 0f) // 점령 중도 아니고 
        {
            Current_Gauge -= Time.deltaTime * occu_Speed;
            yield return null;
        }
        
        yield return null;
    }


}
