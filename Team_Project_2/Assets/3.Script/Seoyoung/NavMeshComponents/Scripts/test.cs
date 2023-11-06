using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
	[SerializeField]
	private GameObject _mapPrefab;

	
	private void Awake()
    {
        GenerateNavmesh();
    }

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
