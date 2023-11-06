using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void GenerateNavmesh()
    {

        NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

        foreach (var s in surfaces)
        {
            s.RemoveData();
            s.BuildNavMesh();
        }

    }
}
