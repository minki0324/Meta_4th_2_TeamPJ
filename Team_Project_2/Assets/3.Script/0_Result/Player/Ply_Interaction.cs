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
    // 하루종일 쳐자서 죄송합니다,,

    [SerializeField] private Occupation occupation; // 점령 스크립트
    [SerializeField] private DoorInter Doorinter;   // 문 열기 스크립트
    [SerializeField] private Text Doorui; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            occupation.ObjEnable(true);

            StopCoroutine(occupation.UnOccu_co());
            StartCoroutine(occupation.Occu_co());
        }
        if (other.gameObject.CompareTag("Door"))
        {
            Doorui.gameObject.SetActive(true);
            StartCoroutine(Doorinter.OpenDoor_co());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {

            occupation.ObjEnable(false);
            StopCoroutine(occupation.Occu_co());
            StartCoroutine(occupation.UnOccu_co());
        }
        if (other.gameObject.CompareTag("Door"))
        {
            Doorui.gameObject.SetActive(false); 
            StopCoroutine(Doorinter.OpenDoor_co());
        }
    }
    
}
