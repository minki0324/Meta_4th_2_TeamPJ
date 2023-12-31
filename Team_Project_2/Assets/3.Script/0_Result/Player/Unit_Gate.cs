using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Gate : MonoBehaviour
{
    // 플레이어 상호작용

    // 점령
    // 1. 점령 후 깃발 색 변경
    // 2. 점령 후 포인트가 속도 변경
    // 3. 주변 유닛 수에 따른 점령 슬라이더 변경

    private GameObject gateUI; // 문 열기 Text
    public bool isMyGate = false;

    private void Start()
    {
        gateUI = GameObject.Find("GateUI");
        gateUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gate") && gameObject.CompareTag("Player"))
        {
            gateUI.gameObject.SetActive(true);  // gateUI 활성화 
            isMyGate = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Gate") && gameObject.CompareTag("Player")) 
        {
            gateUI.gameObject.SetActive(false);  // gateUI 비활성화        
            isMyGate = false;
        }
    }
}
