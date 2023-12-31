using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleProceduralTerrainProject;

public abstract class Targetsetting : MonoBehaviour
{
    [HideInInspector] public TerrainGenerator MapInfo;
    [HideInInspector] public Targetsetting Targetset;

    [HideInInspector] public List<GameObject> ListTemp;
    
    [HideInInspector] public GameObject TargetBase;
    

    [HideInInspector] public Gate[] gates;         // 게이트들 담을 거
    [HideInInspector] public Gate TargetGate;      // 내가 갈 게이트
    [HideInInspector] public float Distance;       // 게이트와 나의 거리

    private void Start()
    {
        ListTemp = new List<GameObject>();
        MapInfo = FindObjectOfType<TerrainGenerator>();
        gates = FindObjectsOfType<Gate>();
        
    }

    public abstract Transform Target(Transform StartPos);

}
