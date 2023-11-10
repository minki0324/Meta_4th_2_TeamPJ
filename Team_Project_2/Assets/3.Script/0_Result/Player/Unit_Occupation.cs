using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Occupation : MonoBehaviour
{
    // 유닛의 점령 상호작용
    

    private Flag flag; // 깃발 스크립트
    public OccupationHUD OccuHUD;
    public int Flag_Num=0;

    private void Awake()
    {
        OccuHUD = FindObjectOfType<OccupationHUD>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            flag = other.gameObject.GetComponentInChildren<Flag>();            
            flag.unit_O = this;
            flag.isOccupating = true;

            for (int i = 0; i < OccuHUD.FlagArray.Length; i++)
            {
                if (flag.Equals(OccuHUD.FlagArray[i]))
                {
                    Flag_Num = i;
                    break;
                }
            }

            if (!gameObject.layer.Equals(7)) // 레이어가 플레이어가 아닐 때
            {                
                StartCoroutine(flag.OnOccu_co(6, this.gameObject.layer));
            }
            else // 레이어가 플레이얼 때   --> 코루틴 내에서 예외처리해서 나중에 if-else문 지워도 됨.
            {
                StartCoroutine(flag.OnOccu_co(11, this.gameObject.layer));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            flag.isOccupating = false;
            StartCoroutine(flag.OffOccu_co(this.gameObject.layer));
        }
    }
}
