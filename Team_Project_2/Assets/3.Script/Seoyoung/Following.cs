using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following : MonoBehaviour
{

    private List<GameObject> nearestMinion_List = new List<GameObject>();
    public LayerMask TargetLayer;

   // private UnityEngine.AI.NavMeshAgent[] agents;

    private Player_Controller pc;
    private Minion_Controller[] minionController;
    GameObject shortobj = new GameObject();
    private void Awake()
    {
        pc = FindObjectOfType<Player_Controller>();
    }

    private void Start()
    {
        StartCoroutine(Mode_Follow_co());
    }

    private bool isTarget
    {
        get
        {
            if (!pc.isDead)
            {
                return true;
            }
            return false;
        }

    }

    public IEnumerator Mode_Follow_co()
    {

        minionController = GetComponentsInChildren<Minion_Controller>();
        // agents = GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>();
        
        while (true)
        {
            if (isTarget)
            {
                for (int i = 0; i < pc.Minions_List.Count; i++)
                {
                    pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
                }

                //수정중..이 망할놈들이 추가될 때도 작동하도록 변경..

                //agents[0].SetDestination(transform.position + Vector3.back);


                GameObject FollowObj = pc.gameObject;
                for (int i = 0; i < pc.Minions_List.Count; i++)
                {

                    float ShortDis = Vector3.Distance(FollowObj.transform.position, pc.Minions_List[0].transform.position);
                    for (int j = 0; j < pc.Minions_List.Count; j++)
                    {
                        float Distance = Vector3.Distance(FollowObj.transform.position, pc.Minions_List[j].transform.position);
                        if (ShortDis >= Distance)
                        {
                            // nearestMinion_List.Add(pc.Minions_List[j]);


                            ShortDis = Distance;
                            shortobj = pc.Minions_List[j];
                        }

                        //if (j == 0)
                        //{
                        //    nearestMinion_List[j].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position);
                        //}
                        //else
                        //{
                        //    nearestMinion_List[j].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nearestMinion_List[j - 1].transform.position + Vector3.back);
                        //}

                        //FollowObj = nearestMinion_List[j];
                    }
                    nearestMinion_List.Add(shortobj);
                    if (i == 0)
                    {
                        nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position);
                    }
                    else
                    {
                        nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nearestMinion_List[i - 1].transform.position + Vector3.back);
                    }

                    FollowObj = nearestMinion_List[i];

                }








            }



            yield return null;

        }


    }




    public void Swap(ref float a ,ref float b)
    {
        float temp = a;
        a = b;
        b = temp;

    }
}
