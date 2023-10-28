using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    private Player_Controller playerController;

    private Minion_Controller minionController;

    private Player_Movement playerMovement;

    private UnityEngine.AI.NavMeshAgent agent;

   

    public LayerMask TargetLayer;



    private List<GameObject> nearestMinion_List = new List<GameObject>();



    private void Awake()
    {
        playerController = FindObjectOfType<Player_Controller>();
        minionController = GetComponent<Minion_Controller>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerMovement = FindObjectOfType<Player_Movement>();
    }
    void Start()
    {
        agent.velocity = Vector3.zero;
        agent.avoidancePriority = 1;
        StartCoroutine(Mode_Follow_co());
    }



    private bool isTarget
    {
        get
        {
            if (playerController != null && !playerController.isDead)
            {
                return true;
            }
            return false;
        }

    }
    public IEnumerator Mode_Follow_co()
    {
       


        while (!minionController.isDead)
        {
            if (isTarget)
            {
                agent.isStopped = false;
                //¼öÁ¤Áß..

                //  agent.SetDestination(playerController.transform.position + Vector3.back);
                //Debug.Log(playerController.transform.position);
                //for (int i = 0; i < playerController.Minions_List.Count; i++)
                //{


                //float ShortDis = Vector3.Distance(playerController.gameObject.transform.position, playerController.Minions_List[0].transform.localPosition);
                //for (int j = 1; j < playerController.Minions_List.Count; j++)
                //{
                //    float Distance = Vector3.Distance(playerController.gameObject.transform.position, playerController.Minions_List[j].transform.localPosition);
                //    if (ShortDis >= Distance)
                //    {
                //        nearestMinion_List.Add(playerController.Minions_List[j]);
                //        ShortDis = Distance;
                //    }


                //}

                //for (int j = 0; j < nearestMinion_List.Count; j++)
                //{
                //    if (j == 0)
                //    {
                //        agent.SetDestination(playerController.transform.position + Vector3.back);
                //        //nearestMinion_List[0].transform.position = playerController.transform.position + Vector3.back;
                //    }
                //    else
                //    {
                //        agent.SetDestination(nearestMinion_List[j - 1].transform.position + Vector3.back);
                //        // nearestMinion_List[i].transform.position = nearestMinion_List[i - 1].transform.position + Vector3.back;
                //    }
                //}
                




                //}


            }
            else
            {

                agent.isStopped = true;
                Collider[] cols = Physics.OverlapSphere(transform.position, 20f, TargetLayer);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].TryGetComponent(out Player_Controller p))
                    {
                        if (!p.isDead)
                        {
                            playerController = p;
                            break;
                        }
                    }

                }
            }

            yield return null;
        }

    }

}
