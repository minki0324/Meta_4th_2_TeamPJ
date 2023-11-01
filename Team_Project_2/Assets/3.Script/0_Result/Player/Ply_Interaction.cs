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


   [SerializeField] private Occupation occupation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Flag"))
        {
            occupation.ObjEnable(true);

            StopCoroutine(occupation.UnOccu_co());
            StartCoroutine(occupation.Occu_co());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Flag"))
        {
            
            occupation.ObjEnable(false);
            StopCoroutine(occupation.Occu_co());
            StartCoroutine(occupation.UnOccu_co());
        }
    }
    


    
    
    
}
