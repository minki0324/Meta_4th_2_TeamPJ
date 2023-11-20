using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Object/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("기본 정보")]
    public int Upgrade_Index;
    public string Upgrade_Name;
    [TextArea]
    public string Upgrade_Des;
    public int Upgrade_Cost;
    public Sprite Upgrade_Icon;

    [Header("업글 정보")]
    [TextArea]
    public string Des;
    public float Value;
}
