using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(Team.Team1);
            
        }
    }
}
