using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ply_Interaction : MonoBehaviour
{
    // 플레이어 상호작용

    // 점령
    // 1. 점령 후 깃발 색 변경
    // 2. 점령 후 포인트가 속도 변경
    // 3. 주변 유닛 수에 따른 점령 슬라이더 변경

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject OpenDoorui;



    // 점령관련 변수들
    [SerializeField] Slider OccuValue; // 점령 게이지
    private float Num_Person = 1.03f; // 사람 수에 따른 배율
    public float occu_Speed = 15f; // 점령 속도
    private float Total_Gauge = 100f; // 전체 점령 게이지
    private float Current_Gauge = 0;  // 현재 점령 게이지
    bool isOccupating = false; // 점령 중인지


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Flag"))
        {
            OccuValue.gameObject.SetActive(true);
            isOccupating = true;
            StartCoroutine(Interacting());
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Flag"))
        {
            isOccupating = false;
            StopCoroutine(Interacting());
            StartCoroutine(noInteracting());
        }
    }
    private IEnumerator Interacting()
    {

        // 점령 중
        while (isOccupating && Current_Gauge <= 100f)
        {
            Current_Gauge += Time.deltaTime * occu_Speed; // 나중에 인원수에 따른 배율 넣어야해용
            Debug.Log(Current_Gauge);
            OccuValue.value = Current_Gauge / Total_Gauge;
            yield return null; 
        }

    }
    private IEnumerator noInteracting()
    {
        OccuValue.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        while (!isOccupating && Current_Gauge >= 0f)
        {
            Current_Gauge -= Time.deltaTime * occu_Speed;
            OccuValue.value = Current_Gauge / Total_Gauge;


            yield return null;
        }
    }


    
    
    
}
