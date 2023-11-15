using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Occupation : MonoBehaviour
{ 
    // 유닛의 점령 상호작용   

    private Flag flag; // 내 감지범위에 닿은 깃발
    private int Team_Color;  // 팀 Color

    private void Start()
    {
        switch (this.gameObject.layer)
        {
            case (int)TeamLayerIdx.Player:
                Team_Color = GameManager.instance.Color_Index;
                break;
            case (int)TeamLayerIdx.Team1:
                Team_Color = GameManager.instance.T1_Color;
                break;
            case (int)TeamLayerIdx.Team2:
                Team_Color = GameManager.instance.T2_Color;
                break;
            case (int)TeamLayerIdx.Team3:
                Team_Color = GameManager.instance.T3_Color;
                break;
            default:
                return;
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            flag = other.gameObject.GetComponentInChildren<Flag>();            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            flag = null;
        }
    }
}
