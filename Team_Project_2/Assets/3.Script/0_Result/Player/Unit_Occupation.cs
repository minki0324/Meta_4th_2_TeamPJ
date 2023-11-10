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
            flag.isOccupating = true;
            flag.unit_O = this;

            for (int i = 0; i < OccuHUD.FlagArray.Length; i++)
            {
                if (flag.Equals(OccuHUD.FlagArray[i]))
                {
                    Flag_Num = i;
                    break;
                }
                ;
            }

            if (gameObject.layer.Equals(8))
            {
                
                StartCoroutine(flag.OnOccu_co());

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            if (gameObject.layer.Equals(8))
            {

                StartCoroutine(flag.OffOccu_co());

            }

        }
        //Flag_Num = 0;
    }



}
