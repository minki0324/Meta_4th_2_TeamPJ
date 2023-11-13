using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour
{
    [SerializeField] private GameObject[] colorChips;

    public void Change_ColorChip()
    {
        ColorManager.instance.Change_SolidColor(colorChips[0], GameManager.instance.Color_Index);
        ColorManager.instance.Change_SolidColor(colorChips[1], GameManager.instance.T1_Color);
        ColorManager.instance.Change_SolidColor(colorChips[2], GameManager.instance.T2_Color);
        ColorManager.instance.Change_SolidColor(colorChips[3], GameManager.instance.T3_Color);
    }
}
