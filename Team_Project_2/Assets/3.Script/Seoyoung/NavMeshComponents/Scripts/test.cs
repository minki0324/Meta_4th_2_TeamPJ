using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
	[SerializeField]
	private GameObject Plane1;

	[SerializeField]
	NavMeshSurface[] surfaces;


	private void Awake()
    {
       // GenerateNavmesh();
    }

	private void GenerateNavmesh()
	{

		surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

		foreach (var s in surfaces)
		{
			s.RemoveData();
			s.BuildNavMesh();
		}

	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			GenerateNavmesh();
		}
	}

}
