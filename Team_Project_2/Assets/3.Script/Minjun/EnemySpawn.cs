using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : LeaderState
{

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        //같은팀의 깃발 근처에가면 canSpawn =true;
        if(maxUnitCount <= currentUnitCount)
        {
            canSpawn = false;
        }





        if(Gold >= unitCost && canSpawn)
        {

        }
    }


}
