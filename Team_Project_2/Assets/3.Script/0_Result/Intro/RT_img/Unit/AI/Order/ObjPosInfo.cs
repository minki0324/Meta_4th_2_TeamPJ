using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleProceduralTerrainProject;

public class ObjPosInfo : MonoBehaviour
{
    private TerrainGenerator MapInfo;

    public Transform[] Bases;
    public Transform[] Flags;


    private void Start()
    {
        MapInfo = FindObjectOfType<TerrainGenerator>();

        Bases = new Transform[MapInfo.baseCampPositions.Count];
        Flags = new Transform[MapInfo.flagPositions_List.Count];

        for (int i = 0; i < MapInfo.baseCampPositions.Count; i++)
        {        
            Bases[i] = MapInfo.baseCampPositions[i].transform;
        }

        for (int i = 0; i < MapInfo.flagPositions_List.Count; i++)
        {
            Flags[i] = MapInfo.flagPositions_List[i].transform;
        }

    }

}
