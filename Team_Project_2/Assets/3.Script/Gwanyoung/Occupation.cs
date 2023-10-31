using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Occupation : MonoBehaviour
{
    // 점령
    // 1. 점령 후 깃발 색 변경
    // 2. 점령 후 포인트가 속도 변경
    // 3. 주변 유닛 수에 따른 점령 슬라이더 변경


    [Header("깃발")]
    [SerializeField] GameObject Flag; // 깃발의 문양 오브젝트


    SkinnedMeshRenderer skinnedmesh;

    [Header("적용할 컬러배열(Material)")]
    [SerializeField] private Material[] Flag_Color; // 깃발 색바꿀 Marterial

    public float Num_Person = 1.03f; // 사람 수에 따른 배율
    public float occu_Speed = 5f; // 점령 속도

    public float Total_Gauge = 100f;
    public float Current_Gauge = 0;


    private void Start()
    {
        skinnedmesh = Flag.GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
      //  OccuValue.value = Current_Gauge / Total_Gauge;
        //skinnedmesh.material = Flag_Color[0];
    }
 


}
