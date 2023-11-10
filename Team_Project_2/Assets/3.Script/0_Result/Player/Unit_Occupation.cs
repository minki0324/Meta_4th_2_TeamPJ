using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Occupation : MonoBehaviour
{ 
    // À¯´ÖÀÇ Á¡·É »óÈ£ÀÛ¿ë   
    private Flag flag; // ±ê¹ß ½ºÅ©¸³Æ®
    [HideInInspector] public OccupationHUD OccuHUD; // Á¡·ÉHUD

    [HideInInspector] public int Flag_Num = 0;  // Flag ÀÎµ¦½º 

    public int Team_Color;  // ÆÀ Color

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

        OccuHUD = FindObjectOfType<OccupationHUD>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            flag = other.gameObject.GetComponentInChildren<Flag>();            
            flag.unit_O = this;
            flag.isOccupating = true; // Á¡·É Áß true

            for (int i = 0; i < OccuHUD.FlagArray.Length; i++)
            {
                if (flag.Equals(OccuHUD.FlagArray[i]))
                {
                    Flag_Num = i;
                    break;
                }
            }
            StartCoroutine(flag.OnOccu_co(Team_Color, this.gameObject.layer));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            flag.isOccupating = false; // Á¡·É Áß false
            StartCoroutine(flag.OffOccu_co(this.gameObject.layer));
            flag.unit_O = null;
        }
    }
}
