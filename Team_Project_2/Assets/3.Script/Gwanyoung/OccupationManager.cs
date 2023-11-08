using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupationManager : MonoBehaviour
{
    // 1. 점령 후 포인트가 속도 변경
    // 2. 주변 유닛 수에 따른 점령 슬라이더 변경

    public Flag[] FlagArray;  // 플래그 컴포넌트 배열

    [HideInInspector] public float Num_Soldier = 1.03f; // 사람 수에 따른 배율
    [HideInInspector] public float occu_Speed = 12f; // 점령 속도


   // private void Awake()
   // {
   //     FlagArray = FindObjectsOfType<Flag>();
   //     
   // }
   // private void Update()
   // {
   //     if (Current_Gauge / Total_Gauge >= 1 && !isOccupied)
   //     {
   //         isOccupied = true;
   //     }
   //
   // }
   // public IEnumerator Occu_co()
   // {
   //     // 점령 중
   //     while (isOccupating && Current_Gauge <= 100f)
   //     {
   //         Current_Gauge += Time.deltaTime * occu_Speed * Num_Soldier; // 나중에 인원수에 따른 배율 넣어야해용
   //         Debug.Log(Current_Gauge);
   //         yield return null;
   //     }
   //
   // }
   // public IEnumerator UnOccu_co()
   // {
   //     yield return new WaitForSeconds(3.0f);
   //     while (!isOccupied && !isOccupating && Current_Gauge >= 0f)
   //     {
   //         Current_Gauge -= Time.deltaTime * occu_Speed;
   //         yield return null;
   //     }
   // }


}
