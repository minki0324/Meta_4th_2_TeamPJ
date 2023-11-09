using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Occupation : MonoBehaviour
{
    // 유닛의 점령 상호작용
    

    private Flag flag; // 깃발 스크립트
    private OccupationHUD OccuHUD;
    public int FlagNum=0;

    private void Awake()
    {
        OccuHUD = FindObjectOfType<OccupationHUD>();

    }

    private void OnTriggerEnter(Collider other)
    {
        flag = other.gameObject.GetComponentInChildren<Flag>();

        for (int i = 0; i < OccuHUD.FlagArray.Length; i++)
        {
            
            if (flag.Equals(OccuHUD.FlagArray[i]))
            {
                
                break;
            }
            FlagNum++;
        }


        if (other.gameObject.CompareTag("Flag"))
        {
            if (gameObject.layer.Equals(8))
            {
                OccuHUD.Change_Color((int)Team.Team3, FlagNum);
                flag.transform.parent.gameObject.layer = this.gameObject.layer;
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
        FlagNum = 0;
    }



}
