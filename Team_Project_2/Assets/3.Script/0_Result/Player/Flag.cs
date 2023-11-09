using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // 점령
    // 1. 점령 후 깃발 색 변경       

    public float Total_Gauge = 100f; // 전체 점령 게이지
    public float Current_Gauge = 0;  // 현재 점령 게이지

    public bool isOccupating = false; // 점령 중인지
    public bool isOccupied = false; // 점령이 끝났는지
    private SkinnedMeshRenderer skinnedmesh;

    private void Awake()
    {
        TryGetComponent<SkinnedMeshRenderer>(out skinnedmesh);
    }
    public void Change_Flag_Color(int TeamNum)
    { 
        skinnedmesh.material = ColorManager.instance.Flag_Color[TeamNum];        
    }
}
