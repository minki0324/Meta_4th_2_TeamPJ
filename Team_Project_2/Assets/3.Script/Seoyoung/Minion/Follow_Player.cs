using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    private Player_Controller playerController;

    private Minion_Controller minionController;

    private UnityEngine.AI.NavMeshAgent agent;

   

    public LayerMask TargetLayer;



    private List<GameObject> nearestMinion_List = new List<GameObject>();



    private void Awake()
    {
        playerController = FindObjectOfType<Player_Controller>();
        minionController = GetComponent<Minion_Controller>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    void Start()
    {
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

                agent.SetDestination(playerController.transform.position + Vector3.back);
                Debug.Log(playerController.transform.position);
            }
            else
            {
                

                for(int i = 0; i<playerController.Minions_List.Count; i++)
                {
                    for (var node = playerController.Minions_List.First; node != null; node = node.Next)
                    {
                        float ob1_distance = Vector3.Magnitude(node.Value.transform.position);
                        float ob2_distance = Vector3.Magnitude(node.Next.Value.transform.position);

                        if (ob1_distance < ob2_distance)
                        {
                            nearestMinion_List.Add(node.Value);
                        }
                        else
                        {
                            nearestMinion_List.Add(node.Next.Value);
                            // nearestMinion = node.Next.Value;
                        }

                        // node.Value.transform.position = node.Next.Value.transform.position + Vector3.back;

                    }
                    if (i > 0)
                    {
                        nearestMinion_List[i].transform.position = nearestMinion_List[i - 1].transform.position + Vector3.back;
                    }
                   
                }

             

                
              

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
