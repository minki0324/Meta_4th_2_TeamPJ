using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "UnitInfo", menuName = "Unit Info")]
public class Unit_Information : ScriptableObject
{

    public string unitName;
    public string description;
    public Sprite icon;
    public float damage;
    public float maxHP;
    public float currentHP;
    public float attackRange;
    // 다른 정보 필드들을 추가할 수 있습니다.

}
