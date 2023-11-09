using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InitNavMesh : MonoBehaviour
{
	[SerializeField]
	NavMeshSurface[] surfaces;


	private void Awake()
	{
		// GenerateNavmesh();
	}

	public void GenerateNavmesh()
	{

		surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

		foreach (var s in surfaces)
		{
			s.RemoveData();
			s.BuildNavMesh();
		}

	}
	
}
