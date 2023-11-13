using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleProceduralTerrainProject;

public class StateOccuFlag : MonoBehaviour, IState
{
    public TerrainGenerator MapInfo;
    //public NavMeshAgent Nav;


    private void Start()
    {
       
        MapInfo = FindObjectOfType<TerrainGenerator>();
    }
    public void OperEnter()
    {
        
       
    }

    public void OperExit()
    {
    }

    public void OperStay()
    {
        NavMeshAgent Nav = this.GetComponent<NavMeshAgent>();
        Debug.Log(Nav);
        //Nav.SetDestination(MapInfo.flagPositions_List[Random.Range(0, MapInfo.flagPositions_List.Count)].transform.position);
    }

}
