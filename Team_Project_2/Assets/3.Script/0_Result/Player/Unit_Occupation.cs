using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Occupation : MonoBehaviour
{
    // 유닛의 점령 상호작용
    

    private Flag flag; // 깃발 스크립트
    private OccupationHUD OccuHUD;

    private void Awake()
    {
        OccuHUD = FindObjectOfType<OccupationHUD>();

    }

    private void OnTriggerEnter(Collider other)
    {
        flag = other.gameObject.GetComponentInChildren<Flag>();       
        if (other.gameObject.CompareTag("Flag"))
        {
            if (gameObject.CompareTag("Player"))
            {
                
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            if (gameObject.CompareTag("Player"))
            {
                
            }

        }
    }

    private void Change_Color(int Coloridx)
    {
        flag.Change_Flag_Color(Coloridx);
        OccuHUD.Change_HUD_Color(Coloridx);
    }
    
}
